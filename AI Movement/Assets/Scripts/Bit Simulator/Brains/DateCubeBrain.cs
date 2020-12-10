using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{


    public class DataCubebrain : MonoBehaviour
    {
        public enum DataCubeMode
        {
            Idle,
            SearchForMemory,
            SearchForProcessorTree,
            RunFromEnemys,
        }

        public enum BehaviorEnum
        {
            Keyboard,
            SeekClickPoint,
            Seek,
            Flee,
            Pursue,
            Evade,
            Wander,
            FollowPath,
            Hide,
            NotSet,
            ObjectAvoid,
        };

        [Header("Manual")]
        public BehaviorEnum m_IdleBehavoir;

        public BehaviorEnum m_SearchForMemoryBehavoir;
        public float SearchForMemoryDistance;

        public BehaviorEnum m_SearchForProssecorTreeBehavoir;
        public float SearchForProssecorTreeDistance;

        public BehaviorEnum m_RunFromEnemyBehavoir;
        public float RunFromEnemyDistance;

        public GameObject m_Target;

        [Header("Private")]
        private Steering m_Steering;

        Dictionary<DataCubeMode, BehaviorEnum> m_DataCubeBehavoirs;

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
                    m_DataCubeMode = value;
                    UpdateHunterBehavoir();
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
            m_Steering = GetComponent<Steering>();

            m_DataCubeBehavoirs = new Dictionary<DataCubeMode, BehaviorEnum>();

            m_DataCubeBehavoirs.Add(DataCubeMode.Idle, m_IdleBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.SearchForMemory, m_SearchForMemoryBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.SearchForProcessorTree, m_SearchForProssecorTreeBehavoir);
            m_DataCubeBehavoirs.Add(DataCubeMode.RunFromEnemys, m_RunFromEnemyBehavoir);

            UpdateHunterBehavoir();
        }

        private void Update()
        {
            CheckBehavoirConditions();
        }

        private void CheckBehavoirConditions()
        {
            switch (DataCubeStatus)
            {
                case DataCubeMode.Idle:
                    break;
                case DataCubeMode.SearchForMemory:
                    break;
                case DataCubeMode.SearchForProcessorTree:
                    break;
                case DataCubeMode.RunFromEnemys:
                    break;
            }
        }

        private bool CheckForEnemys()
        {
            bool _virius = false;
            bool _CC = false;
            ViriusBrain[] viriusBrains = Support.CheckForNearbyObjects<ViriusBrain>(transform.position, RunFromEnemyDistance);
            GarbargeCollectorBrain[] garbargeCollectorBrains = Support.CheckForNearbyObjects<GarbargeCollectorBrain>(transform.position, RunFromEnemyDistance);
            if(viriusBrains != null)
            {
                if(viriusBrains.Length > 0)
                {
                    _virius = true; 
                }
            }
            if(garbargeCollectorBrains != null) 
            {
                if(garbargeCollectorBrains.Length > 0)
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
        private void UpdateHunterBehavoir()
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

                default:
                    Debug.LogError($"Behavior of Type{behaviorEnum} not implemented yet!");
                    break;
            }
            behavors[0].Label = label;
            return behavors;
        }

    }
}

