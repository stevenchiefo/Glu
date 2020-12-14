using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;


public class DataCubeBrain : PoolableObject, IDataBrains
{
    public GameObject m_Target;

    [Header("Private")]
    private Steering2D m_Steering;

    private delegate void DataCubeTask();

    private DataCubeMode m_DataCubeMode;
    private Dictionary<DataCubeMode, BehaviorEnum> m_DataCubeBehavoirs;
    private Dictionary<DataCubeMode, DataCubeTask> m_Tasks;

    public int m_LifeTime;
    public bool m_IsReadyForMating;
    public float m_CurrentMB;
    public float m_CurrentProcess;
    public float Defense;
    public float Speed;
    public DataCubeType DataCube;

    private DataCubeMatingData m_MatingData;
    private DataCubeUI m_DataCubeUI;




    public DataCubeMode DataCubeStatus
    {
        get
        {
            return m_DataCubeMode;
        }
        set
        {
            if (value != m_DataCubeMode)
            {
                m_Target = null;
                m_DataCubeMode = value;
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

        m_DataCubeBehavoirs = new Dictionary<DataCubeMode, BehaviorEnum>();
        m_Tasks = new Dictionary<DataCubeMode, DataCubeTask>();

        m_DataCubeBehavoirs.Add(DataCubeMode.Idle, EnitiyManager.instance.DataCubeSettings.m_IdleBehavoir);
        m_DataCubeBehavoirs.Add(DataCubeMode.SearchForMemory, EnitiyManager.instance.DataCubeSettings.m_SearchForMemoryBehavoir);
        m_DataCubeBehavoirs.Add(DataCubeMode.SearchForProcessorTree, EnitiyManager.instance.DataCubeSettings.m_SearchForProssecorTreeBehavoir);
        m_DataCubeBehavoirs.Add(DataCubeMode.RunFromEnemys, EnitiyManager.instance.DataCubeSettings.m_RunFromEnemyBehavoir);

        m_Tasks.Add(DataCubeMode.Idle, CheckIdle);
        m_Tasks.Add(DataCubeMode.SearchForMemory, CheckSearchForMemory);
        m_Tasks.Add(DataCubeMode.SearchForProcessorTree, CheckForProssecor);
        m_Tasks.Add(DataCubeMode.RunFromEnemys, RunFormEnemy);

        OnSpawn.AddListener(StartAllCoroutines);
        OnPool.AddListener(Stop);

        m_DataCubeUI = GetComponentInChildren<DataCubeUI>();
        m_DataCubeUI.SetBrain(this);

        UpdateDataCubeBehavoir();
    }

    public void Born(DataCubeBrain[] Parents)
    {
        int _Speedindex = Mathf.RoundToInt(Random.Range(0, Parents.Length));
        int _Defenseindex = Mathf.RoundToInt(Random.Range(0, Parents.Length));
        int _DataType = Mathf.RoundToInt(Random.Range(0, Parents.Length));

        if (Parents != null)
        {
            Speed = Parents[_Speedindex].Speed;
            Defense = Parents[_Defenseindex].Defense;
            DataCube = Parents[_DataType].DataCube;
        }

        float futureMb = 0;
        float futureProcess = 0;
        foreach (DataCubeBrain i in Parents)
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
            m_DataCubeUI.UpdateUi();
        }
    }

    private void CheckIdle()
    {
        if (CheckForEnemys())
        {
            DataCubeStatus = DataCubeMode.RunFromEnemys;
            return;
        }

        if (m_CurrentProcess < EnitiyManager.instance.DataCubeSettings.m_MaxProcess * 0.50f)
        {
            DataCubeStatus = DataCubeMode.SearchForProcessorTree;
            return;
        }

        if (m_CurrentMB < EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.50f)
        {
            DataCubeStatus = DataCubeMode.SearchForMemory;
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
                DataCubeBrain dataCubebrain = m_Target.GetComponent<DataCubeBrain>();
                if (dataCubebrain != null)
                {


                    if (dataCubebrain.ReqeustMating(this))
                    {
                        DataCubeBrain[] parents = new DataCubeBrain[]
                        {
                            this,
                            dataCubebrain,
                        };
                        EnitiyManager.instance.MakeaDataCube(parents);
                        DataCubeStatus = DataCubeMode.Idle;
                        m_Target = null;
                        m_IsReadyForMating = false;
                        m_LifeTime = 0;
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
        if (m_Target != null)
        {
            float _distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (_distance < EnitiyManager.instance.DataCubeSettings.SearchForMemoryDistance * 0.5f)
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

                MemorySlot _memorySlot = m_Target.GetComponent<MemorySlot>();
                if (_memorySlot.CanFarm)
                {

                    float _futureMb = m_CurrentMB + _memorySlot.FarmMB(1);
                    if (_futureMb >= EnitiyManager.instance.DataCubeSettings.m_MaxMb)
                    {
                        m_CurrentMB = EnitiyManager.instance.DataCubeSettings.m_MaxMb;
                        DataCubeStatus = DataCubeMode.Idle;
                        m_Target = null;
                    }
                    else
                    {
                        m_CurrentMB = _futureMb;
                    }
                    m_DataCubeUI.UpdateUi();
                }
                else
                {
                    DataCubeStatus = DataCubeMode.Idle;
                    m_Target = null;
                }
            }
        }
        else
        {

            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.DataCubeSettings.SearchForMemoryDistance, EnitiyManager.instance.DataCubeSettings.MemoryLayerMask);
            if (collider2Ds.Length > 0)
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
                        m_Target = null;
                    }
                    else
                    {
                        m_CurrentProcess = _futureProcess;
                    }
                    m_DataCubeUI.UpdateUi();
                }
            }
            else
            {
                DataCubeStatus = DataCubeMode.Idle;
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
            if (_distance * 2> EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance)
            {
                DataCubeStatus = DataCubeMode.Idle;
            }
            UpdateDataCubeBehavoir();
        }
        else
        {
            ViriusBrain[] viriusBrains = Support.CheckForNearbyObjects<ViriusBrain>(transform.position, EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance);
            if (viriusBrains != null)
            {

                if (viriusBrains.Length > 0)
                {
                    m_Target = viriusBrains[0].gameObject;
                    UpdateDataCubeBehavoir();
                }
            }
        }
    }

    private void CheckBehavoirConditions()
    {
        if (m_Tasks.ContainsKey(DataCubeStatus))
        {
            m_Tasks[DataCubeStatus]();
        }

    }

    private bool CheckForEnemys()
    {

        Collider2D[] _colsV = Physics2D.OverlapCircleAll(transform.position, EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance, EnitiyManager.instance.DataCubeSettings.RunFromEnemyLayerMask);

        if (_colsV != null)
        {
            if (_colsV.Length > 0)
            {
                return true;
            }
        }
        DataCubeStatus = DataCubeMode.Idle;
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
            float futureproces = m_CurrentProcess - EnitiyManager.instance.MbCpuSettings.ProcessTakeOff;
            if (futureproces <= 0)
            {
                m_CurrentProcess = 0;
                PoolObject();
            }
            else
            {
                m_CurrentProcess = futureproces;

            }
            m_DataCubeUI.UpdateUi();
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
            m_DataCubeUI.UpdateUi();
        }
    }

    private void UpdateDataCubeBehavoir()
    {
        List<IBehavor> behavors = GetBehavoirs(m_DataCubeBehavoirs[DataCubeStatus]);
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
                    behavors.Add(new Flee(m_Target.transform, EnitiyManager.instance.DataCubeSettings.RunFromEnemyDistance));
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
    public bool ReqeustMating(DataCubeBrain dataCubebrain)
    {
        if (m_IsReadyForMating)
        {
            bool Type = dataCubebrain.DataCube == DataCube;
            bool _mb = dataCubebrain.m_CurrentMB >= m_MatingData.NeededCurrentMB;
            bool _Process = dataCubebrain.m_CurrentProcess >= m_MatingData.NeededProcess;
            return _mb == _Process == Type;
        }
        return false;
    }

    public void SetRandomStats()
    {
        Speed = Random.Range(0.1f, EnitiyManager.instance.DataCubeSettings.MaxSpeed);
        Defense = Random.Range(0.1f, EnitiyManager.instance.DataCubeSettings.MaxDefense);
        m_CurrentMB = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxMb));
        m_CurrentProcess = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxProcess));
        m_MatingData = new DataCubeMatingData
        {
            NeededCurrentMB = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxMb)),
            NeededProcess = Mathf.RoundToInt(Random.Range(EnitiyManager.instance.DataCubeSettings.m_MaxMb * 0.5f, EnitiyManager.instance.DataCubeSettings.m_MaxProcess)),
        };
    }

    public int StealMB(int ammount)
    {
        float multiplyer = 1 - (Defense / 10);
        int stealed = Mathf.RoundToInt(ammount * multiplyer);
        float futureMB = m_CurrentMB - stealed;
        float leftover = stealed - m_CurrentMB;
        if (futureMB < 0)
        {
            m_CurrentMB = 0;
            return Mathf.RoundToInt(leftover);
        }
        m_CurrentMB = futureMB;
        return Mathf.RoundToInt(stealed);
    }
}

public struct DataCubeMatingData
{
    public float NeededCurrentMB;
    public float NeededProcess;
}

