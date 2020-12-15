using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class GarbargeCollectorBrain : PoolableObject
{
    public GameObject m_Target;

    [Header("Private")]
    private Steering2D m_Steering;

    private delegate void GarbargeCollectorTask();

    private GarbargeCollectorMode m_GarbargeCollectorMode;
    private Dictionary<GarbargeCollectorMode, BehaviorEnum> m_ViriusBehavoirs;
    private Dictionary<GarbargeCollectorMode, GarbargeCollectorTask> m_Tasks;

    public GarbargeCollectorMode GarbargeCollectorMode
    {
        get
        {
            return m_GarbargeCollectorMode;
        }
        set
        {
            if (value != m_GarbargeCollectorMode)
            {
                m_GarbargeCollectorMode = value;
                UpdateDataCubeBehavoir();
            }
        }
    }

    [Header("WaitPoints")]
    public List<Transform> m_WaitPoints;

    [Header("Object Avoidance")]
    public LayerMask LayerMask;

    public bool m_ObjectAvoidanceActive;
    public float m_Radius;

    [Header("Arrive")]
    public bool m_ArriveActive;

    public override void Load()
    {
        m_Steering = GetComponent<Steering2D>();

        m_ViriusBehavoirs = new Dictionary<GarbargeCollectorMode, BehaviorEnum>();
        m_Tasks = new Dictionary<GarbargeCollectorMode, GarbargeCollectorTask>();

        m_ViriusBehavoirs.Add(GarbargeCollectorMode.Idle, EnitiyManager.instance.GarbargeCollectorSettings.m_IdleBehavoir);
        m_ViriusBehavoirs.Add(GarbargeCollectorMode.Hunt, EnitiyManager.instance.GarbargeCollectorSettings.HuntBehavoir);

        m_Tasks.Add(GarbargeCollectorMode.Idle, CheckIdle);
        m_Tasks.Add(GarbargeCollectorMode.Hunt, CheckHunt);

        UpdateDataCubeBehavoir();
    }

    private void Update()
    {
        CheckIdle();
        CheckBehavoirConditions();
    }

    /// <summary>
    /// Checks the idle mode
    /// </summary>
    private void CheckIdle()
    {
        m_Target = CheckForPrey();
        if (m_Target != null)
        {
            GarbargeCollectorMode = GarbargeCollectorMode.Hunt;
        }
        else
        {
            GarbargeCollectorMode = GarbargeCollectorMode.Idle;
        }
    }

    /// <summary>
    /// Checks the Hunt mode
    /// </summary>
    private void CheckHunt()
    {
        if (m_Target != null)
        {
            float collectdistance = EnitiyManager.instance.GarbargeCollectorSettings.CollectDistance;
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (distance <= collectdistance)
            {
                IDataBrains dataBrains = m_Target.GetComponent<IDataBrains>();
                if (dataBrains != null)
                {
                    dataBrains.Collect();
                    GarbargeCollectorMode = GarbargeCollectorMode.Idle;
                }
            }
        }
    }

    /// <summary>
    /// Calls the function on the current mode
    /// </summary>
    private void CheckBehavoirConditions()
    {
        m_Tasks[m_GarbargeCollectorMode]();
    }

    /// <summary>
    /// Checks if there are any Prey close
    /// </summary>
    /// <returns></returns>
    private GameObject CheckForPrey()
    {
        Collider2D[] _Cols = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.GarbargeCollectorSettings.HuntRange, EnitiyManager.instance.GarbargeCollectorSettings.EnemyLayerMask);
        if (_Cols.Length > 0)
        {
            ViriusBrain viriusBrain = _Cols[0].GetComponent<ViriusBrain>();
            if (viriusBrain != null)
            {
                return viriusBrain.gameObject;
            }

            DataCubeBrain dataCubeBrain = _Cols[0].GetComponent<DataCubeBrain>();
            if (dataCubeBrain != null)
            {
                if (dataCubeBrain.m_CurrentMB <= EnitiyManager.instance.GarbargeCollectorSettings.NeededMbToHunt)
                {
                    return dataCubeBrain.gameObject;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Update behavoir on current mode
    /// </summary>
    private void UpdateDataCubeBehavoir()
    {
        List<IBehavor> behavors = GetBehavoirs(m_ViriusBehavoirs[GarbargeCollectorMode]);
        ObjectAvoidance objectAvoidance = null;
        Arrive arrive = null;
        if (m_ObjectAvoidanceActive)
        {
            objectAvoidance = new ObjectAvoidance(m_Radius, LayerMask);
            objectAvoidance.Label = BehaviorEnum.ObjectAvoid.ToString();
            behavors.Add(objectAvoidance);
        }
        if (m_ArriveActive)
        {
            if (m_Target != null)
            {
                arrive = new Arrive(m_Target.transform);
                arrive.Label = "Arrive";
                behavors.Add(arrive);
            }
        }
        if (behavors.Count > 0)
        {
            m_Steering.SetBehaviors(objectAvoidance, arrive, behavors, behavors[0].Label);
        }
        else
        {
            m_Steering.SetBehaviors(objectAvoidance, arrive, behavors, "No Behavoir");
        }
    }

    /// <summary>
    /// Change the behavoir manual
    /// </summary>
    /// <param name="behaviorEnum"></param>
    private void SetQuickBehavoir(BehaviorEnum behaviorEnum)
    {
        List<IBehavor> behavors = GetBehavoirs(behaviorEnum);
        ObjectAvoidance objectAvoidance = null;
        Arrive arrive = null;
        if (m_ObjectAvoidanceActive)
        {
            objectAvoidance = new ObjectAvoidance(m_Radius, LayerMask);
            objectAvoidance.Label = BehaviorEnum.ObjectAvoid.ToString();
            behavors.Add(objectAvoidance);
        }
        if (m_ArriveActive)
        {
            arrive = new Arrive(m_Target.transform);
            arrive.Label = "Arrive";
            behavors.Add(arrive);
        }
        m_Steering.SetBehaviors(objectAvoidance, arrive, behavors, behavors[0].Label);
    }

    /// <summary>
    /// Get behavoir list
    /// </summary>
    /// <param name="behaviorEnum"></param>
    /// <returns></returns>
    private List<IBehavor> GetBehavoirs(BehaviorEnum behaviorEnum)
    {
        List<IBehavor> behavors = new List<IBehavor>();
        string label = behaviorEnum.ToString();
        switch (behaviorEnum)
        {
            case BehaviorEnum.Keyboard:
                behavors.Add(new KeyBoard());
                break;

            case BehaviorEnum.SeekClickPoint:
                behavors.Add(new ClickSeekPoint());
                break;

            case BehaviorEnum.Seek:
                behavors.Add(new Seek(m_Target.transform));
                break;

            case BehaviorEnum.Flee:
                if (m_Target != null)
                {
                    behavors.Add(new Flee(m_Target.transform));
                }
                break;

            case BehaviorEnum.FollowPath:
                behavors.Add(new FollowPath(m_WaitPoints));
                break;

            case BehaviorEnum.Pursue:
                behavors.Add(new Persue(m_Target.transform));
                break;

            case BehaviorEnum.Evade:
                behavors.Add(new Evade(m_Target.transform));
                break;

            case BehaviorEnum.Wander:
                behavors.Add(new Wander2D());
                break;

            case BehaviorEnum.Ilde:
                behavors.Add(new Idle());
                break;

            default:
                Debug.LogError($"Behavior of Type{behaviorEnum} not implemented yet!");
                break;
        }
        if (behavors.Count > 0)
        {
            behavors[0].Label = label;
        }
        return behavors;
    }
}