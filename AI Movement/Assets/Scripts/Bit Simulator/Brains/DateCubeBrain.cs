using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public class DataCubebrain : MonoBehaviour
    {
        [SerializeField] private DataCubeSettings m_Settings;

        public GameObject m_Target;

        [Header("Private")]
        private Steering2D m_Steering;

        private bool m_IsReadyForMating;

        private delegate void DataCubeTask();

        private Dictionary<DataCubeMode, BehaviorEnum> m_DataCubeBehavoirs;
        private Dictionary<DataCubeMode, DataCubeTask> m_Tasks;

        private DataCubeMode m_DataCubeMode;

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

        private void Start()
        {
            m_Steering = GetComponent<Steering2D>();

            m_DataCubeBehavoirs = new Dictionary<DataCubeMode, BehaviorEnum>();

            m_DataCubeBehavoirs.Add(DataCubeMode.Idle, m_Settings.m_IdleBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.SearchForMemory, m_Settings.m_SearchForMemoryBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.SearchForProcessorTree, m_Settings.m_SearchForProssecorTreeBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.RunFromEnemys, m_Settings.m_RunFromEnemyBehavoir);

            m_Tasks.Add(DataCubeMode.Idle, CheckIdle);
            m_Tasks.Add(DataCubeMode.SearchForMemory, CheckSearchForMemory);
            m_Tasks.Add(DataCubeMode.SearchForProcessorTree, CheckForProssecor);

            UpdateDataCubeBehavoir();
        }

        private void Update()
        {
            CheckBehavoirConditions();
        }

        private void CheckIdle()
        {
        }

        private void CheckSearchForMemory()
        {
            if (!m_Target)
            {
                float _distance = Vector3.Distance(transform.position, m_Target.GetComponent<MemorySlot>().GetClosestPoint(transform.position));
                if (_distance < m_Steering.m_Settings.m_ArriveDistance)
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
                }
            }
            else
            {
                MemorySlot[] memorySlots = Support.CheckForNearbyObjects<MemorySlot>(transform.position, m_Settings.SearchForMemoryDistance);
                if (memorySlots.Length > 0)
                {
                    m_Target = memorySlots[0].gameObject;
                }
            }
        }

        private void CheckForProssecor()
        {
            if (!m_Target)
            {
                float _distance = Vector3.Distance(transform.position, m_Target.GetComponent<MemorySlot>().GetClosestPoint(transform.position));
                if (_distance < m_Steering.m_Settings.m_ArriveDistance)
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
                }
            }
            else
            {
                MemorySlot[] memorySlots = Support.CheckForNearbyObjects<MemorySlot>(transform.position, m_Settings.SearchForMemoryDistance);
                if (memorySlots.Length > 0)
                {
                    m_Target = memorySlots[0].gameObject;
                }
            }
        }

        private void RunFormEnemy()
        {
        }

        private void CheckBehavoirConditions()
        {
            if (!CheckForEnemys())
            {
                m_Tasks[m_DataCubeMode].BeginInvoke(null, null);
            }
            else
            {
            }
            UpdateDataCubeBehavoir();
        }

        private bool CheckForEnemys()
        {
            bool _virius = false;
            bool _CC = false;
            ViriusBrain[] viriusBrains = Support.CheckForNearbyObjects<ViriusBrain>(transform.position, m_Settings.RunFromEnemyDistance);
            GarbargeCollectorBrain[] garbargeCollectorBrains = Support.CheckForNearbyObjects<GarbargeCollectorBrain>(transform.position, m_Settings.RunFromEnemyDistance);
            if (viriusBrains != null)
            {
                if (viriusBrains.Length > 0)
                {
                    _virius = true;
                }
            }
            if (garbargeCollectorBrains != null)
            {
                if (garbargeCollectorBrains.Length > 0)
                {
                    _CC = true;
                }
            }
            return _virius || _CC;
        }

        private bool InRange(float Range)
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            return distance <= Range;
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
                arrive = new Arrive(m_Target.transform);
                arrive.Label = "Arrive";
                behavors.Add(arrive);
            }

            m_Steering.SetBehaviors(objectAvoidance, arrive, behavors, behavors[0].Label);
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
                    behavors.Add(new Flee(m_Target.transform));
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
                    behavors.Add(new Wander());
                    break;

                case BehaviorEnum.Ilde:
                    behavors.Add(new Idle());
                    break;

                default:
                    Debug.LogError($"Behavior of Type{behaviorEnum} not implemented yet!");
                    break;
            }
            behavors[0].Label = label;
            return behavors;
        }
    }
}