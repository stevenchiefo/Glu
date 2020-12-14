using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class ViriusBrain : PoolableObject
{
    public GameObject m_Target;

    [Header("Private")]
    private Steering2D m_Steering;

    private delegate void DataCubeTask();

    private ViriusMode m_ViriusMode;
    private Dictionary<ViriusMode, BehaviorEnum> m_ViriusBehavoirs;
    private Dictionary<ViriusMode, DataCubeTask> m_Tasks;

    public int m_LifeTime;
    public bool m_IsReadyForMating;
    public float m_CurrentMB;
    public float m_CurrentProcess;
    public float Speed;
    public ViriusType viriusT;

    private DataCubeMatingData m_MatingData;
    private VirusUI m_VirusUI;




    public ViriusMode ViriusStatus
    {
        get
        {
            return m_ViriusMode;
        }
        set
        {
            if (value != m_ViriusMode)
            {
                m_Target = null;
                m_ViriusMode = value;
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

        m_ViriusBehavoirs = new Dictionary<ViriusMode, BehaviorEnum>();
        m_Tasks = new Dictionary<ViriusMode, DataCubeTask>();

        m_ViriusBehavoirs.Add(ViriusMode.Idle, EnitiyManager.instance.ViriusSettings.m_IdleBehavoir);
        m_ViriusBehavoirs.Add(ViriusMode.HuntForMemory, EnitiyManager.instance.ViriusSettings.m_HuntForMemoryBehavoir);
        m_ViriusBehavoirs.Add(ViriusMode.SearchForProcessorTree, EnitiyManager.instance.ViriusSettings.m_SearchForProssecorTreeBehavoir);
        m_ViriusBehavoirs.Add(ViriusMode.RunFromEnemys, EnitiyManager.instance.ViriusSettings.m_RunFromEnemyBehavoir);

        m_Tasks.Add(ViriusMode.Idle, CheckIdle);
        m_Tasks.Add(ViriusMode.HuntForMemory, CheckSearchForMemory);
        m_Tasks.Add(ViriusMode.SearchForProcessorTree, CheckForProssecor);
        m_Tasks.Add(ViriusMode.RunFromEnemys, RunFormEnemy);

        OnSpawn.AddListener(StartAllCoroutines);
        OnPool.AddListener(Stop);

        m_VirusUI = GetComponentInChildren<VirusUI>();
        m_VirusUI.SetBrain(this);

        UpdateDataCubeBehavoir();
    }

    public void Born(ViriusBrain[] Parents)
    {
        int _Speedindex = Mathf.RoundToInt(Random.Range(0, Parents.Length));
        int _Defenseindex = Mathf.RoundToInt(Random.Range(0, Parents.Length));
        int _DataType = Mathf.RoundToInt(Random.Range(0, Parents.Length));

        if (Parents != null)
        {
            Speed = Parents[_Speedindex].Speed;
            viriusT = Parents[_DataType].viriusT;
        }

        float futureMb = 0;
        float futureProcess = 0;
        foreach (ViriusBrain i in Parents)
        {
            futureMb += i.m_CurrentMB;
            futureProcess += i.m_CurrentProcess;
        }
        m_CurrentMB = futureMb / Parents.Length;
        m_CurrentProcess = futureProcess / Parents.Length;
        m_MatingData = new DataCubeMatingData
        {
            NeededCurrentMB = Random.Range(1, EnitiyManager.instance.DataCubeSettings.m_MaxMb),
            NeededProcess = Random.Range(1, EnitiyManager.instance.DataCubeSettings.m_MaxProcess),
        };
        m_Steering.SetSpeed(Speed);
        m_VirusUI.UpdateUi();
    }





    private void Update()
    {
        CheckIdle();
        CheckBehavoirConditions();

    }

    private IEnumerator UpdateLifeTime()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_IsReadyForMating == false);
            yield return new WaitForSeconds(1);
            m_LifeTime++;
            if (m_LifeTime > EnitiyManager.instance.DataCubeSettings.LifeTimeNeeded)
            {
                m_IsReadyForMating = true;
            }
            m_VirusUI.UpdateUi();
        }
    }

    private void CheckIdle()
    {
        if (CheckForEnemys())
        {
            ViriusStatus = ViriusMode.RunFromEnemys;
            return;
        }

        if (m_CurrentProcess < EnitiyManager.instance.ViriusSettings.m_MaxProcess * 0.50f)
        {
            ViriusStatus = ViriusMode.SearchForProcessorTree;
            return;
        }

        if (m_CurrentMB < EnitiyManager.instance.ViriusSettings.m_MaxMb * 0.30f)
        {
            ViriusStatus = ViriusMode.HuntForMemory;
            return;
        }


        if (m_IsReadyForMating)
        {
            CheckForMating();
        }
    }

    private void CheckForMating()
    {
        if (m_Target != null)
        {
            float _distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (_distance < EnitiyManager.instance.DataCubeSettings.MatingDistanceNeeded)
            {
                ViriusBrain _virusBrain = m_Target.GetComponent<ViriusBrain>();
                if (_virusBrain != null)
                {


                    if (_virusBrain.ReqeustMating(this))
                    {
                        ViriusBrain[] parents = new ViriusBrain[]
                        {
                            this,
                            _virusBrain,
                        };
                        EnitiyManager.instance.MakeaDataCube(parents);
                        ViriusStatus = ViriusMode.Idle;
                        m_Target = null;
                        m_IsReadyForMating = false;
                        m_LifeTime = 0;
                        m_VirusUI.UpdateUi();
                    }
                }
            }
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.DataCubeSettings.MatingDistanceNeeded, EnitiyManager.instance.DataCubeSettings.MatingLayerMask);
            if (colliders.Length > 0)
            {
                m_Target = colliders[0].gameObject;
            }

        }
    }

    private void CheckSearchForMemory()
    {
        if(m_Target != null)
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if(distance < EnitiyManager.instance.ViriusSettings.HuntForMemoryDistance * 0.7f)
            {
                DataCubeBrain dataCubeBrain = m_Target.GetComponent<DataCubeBrain>();
                if(dataCubeBrain != null)
                {
                    float futuremb = m_CurrentMB + dataCubeBrain.StealMB(2);
                    if(futuremb <= 0)
                    {
                        m_CurrentMB = 0;
                        ViriusStatus = ViriusMode.Idle;
                        m_Target = null;
                        return;
                    }
                    m_CurrentMB = futuremb;
                    m_VirusUI.UpdateUi();
                }
            }
        }
        else
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.ViriusSettings.HuntForMemoryDistance, EnitiyManager.instance.ViriusSettings.MemoryLayerMask);
            if(collider2Ds.Length > 0)
            {
                m_Target = collider2Ds[0].gameObject;
            }
        }
    }

    private void CheckForProssecor()
    {
        if (m_Target != null)
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (distance < EnitiyManager.instance.DataCubeSettings.SearchForProssecorTreeDistance)
            {
                List<IBehavor> behavors = GetBehavoirs(BehaviorEnum.Seek);
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
                ProcessorOrb processorOrb = m_Target.GetComponent<ProcessorOrb>();
                if (processorOrb != null)
                {
                    float _futureProcess = m_CurrentProcess + processorOrb.FarmOrb(1);
                    if (_futureProcess > EnitiyManager.instance.DataCubeSettings.m_MaxProcess)
                    {

                        m_CurrentProcess = EnitiyManager.instance.DataCubeSettings.m_MaxProcess;
                        ViriusStatus = ViriusMode.Idle;
                        m_Target = null;
                    }
                    else
                    {
                        m_CurrentProcess = _futureProcess;
                    }
                    m_VirusUI.UpdateUi();

                }
            }
            else
            {
                ViriusStatus = ViriusMode.Idle;
            }
        }
        else
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.DataCubeSettings.SearchForProssecorTreeDistance, EnitiyManager.instance.DataCubeSettings.ProcessTreeLayerMask);
            if (collider2Ds.Length > 0)
            {
                m_Target = collider2Ds[0].GetComponent<ProcessorTree>().GetClosestOrb(transform.position).gameObject;
            }
        }
    }

    private void RunFormEnemy()
    {
        if (m_Target != null)
        {
            float _distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (_distance > EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance)
            {
                ViriusStatus = ViriusMode.Idle;
            }
            UpdateDataCubeBehavoir();
        }
        else
        {
            ViriusBrain[] viriusBrains = Support.CheckForNearbyObjects<ViriusBrain>(transform.position, EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance);
            if (viriusBrains.Length > 0)
            {
                m_Target = viriusBrains[0].gameObject;
                UpdateDataCubeBehavoir();
            }
        }
    }

    private void CheckBehavoirConditions()
    {

        m_Tasks[m_ViriusMode]();

    }

    private bool CheckForEnemys()
    {
        Collider2D[] _colsV = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.ViriusSettings.RunFromEnemyDistance, EnitiyManager.instance.ViriusSettings.RunFromEnemyLayerMask);

        if (_colsV != null)
        {
            if (_colsV.Length > 0)
            {
                return true;
            }
        }
        return false;
    }
    private void Stop()
    {
        StopAllCoroutines();
    }

    private void StartAllCoroutines()
    {
        StartCoroutine(TakeOfMb());
        StartCoroutine(TakeOfProcess());
        StartCoroutine(UpdateLifeTime());
    }

    private IEnumerator TakeOfProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnitiyManager.instance.DataCubeSettings.ProcessTimer);
            float futureprocces = m_CurrentProcess - EnitiyManager.instance.MbCpuSettings.ProcessTakeOff;
            if (futureprocces <= 0)
            {
                m_CurrentProcess = 0;
                PoolObject();
            }
            else
            {
                m_CurrentProcess = futureprocces;
            }
            m_VirusUI.UpdateUi();
        }
    }

    private IEnumerator TakeOfMb()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnitiyManager.instance.DataCubeSettings.MBtimer);
            float futureMB = m_CurrentMB - EnitiyManager.instance.MbCpuSettings.MBTakeOff;
            if (futureMB <= 0)
            {
                m_CurrentMB = 0;
            }
            else
            {
                m_CurrentProcess = futureMB;
            }
            m_VirusUI.UpdateUi();
        }
    }

    private void UpdateDataCubeBehavoir()
    {
        List<IBehavor> behavors = GetBehavoirs(m_ViriusBehavoirs[ViriusStatus]);
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
    public bool ReqeustMating(ViriusBrain viriusBrain)
    {
        if (m_IsReadyForMating)
        {
            bool Type = viriusBrain.viriusT == viriusT;
            bool _mb = viriusBrain.m_CurrentMB >= m_MatingData.NeededCurrentMB;
            bool _Process = viriusBrain.m_CurrentProcess >= m_MatingData.NeededProcess;
            return _mb == _Process == Type;
        }
        return false;
    }

    public void SetRandomStats()
    {
        Speed = Random.Range(0.1f, EnitiyManager.instance.DataCubeSettings.MaxSpeed);
        m_CurrentMB = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxMb));
        m_CurrentProcess = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxProcess));
        m_MatingData = new DataCubeMatingData
        {
            NeededCurrentMB = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxMb)),
            NeededProcess = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxProcess)),
        };
    }
}

public struct ViriusMatingData
{
    public float MbNeeded;
    public float ProcessNeeded;
}
