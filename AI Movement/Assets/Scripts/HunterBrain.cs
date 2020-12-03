using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Steering
{
    public class HunterBrain : MonoBehaviour
    {
        public enum HunterMode
        {
            Idle,
            Approach,
            Persue,
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

        public BehaviorEnum m_ApproachBehavoir;
        public float ApproachDistance;

        public BehaviorEnum m_PersueBehavoir;
        public float PersueDistance;

        public GameObject m_Target;

        [Header("Private")]
        private Steering m_Steering;

        Dictionary<HunterMode, BehaviorEnum> m_HunterBehavoirs;

        private HunterMode m_HunterMode;
        public HunterMode HunterStatus
        {
            get
            {
                return m_HunterMode;
            }
            set
            {
                if (value != m_HunterMode)
                {
                    m_HunterMode = value;
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

            m_HunterBehavoirs = new Dictionary<HunterMode, BehaviorEnum>();

            m_HunterBehavoirs.Add(HunterMode.Idle, m_IdleBehavoir);
            m_HunterBehavoirs.Add(HunterMode.Approach, m_ApproachBehavoir);
            m_HunterBehavoirs.Add(HunterMode.Persue, m_PersueBehavoir);

            UpdateHunterBehavoir();
        }

        private void Update()
        {
            CheckHunterDistance();
        }

        private void CheckHunterDistance()
        {
            switch (HunterStatus)
            {
                case HunterMode.Idle:
                    if (InRange(ApproachDistance))
                        HunterStatus = HunterMode.Approach;
                    break;
                case HunterMode.Approach:
                    if (InRange(PersueDistance))
                        HunterStatus = HunterMode.Persue;
                    if (!InRange(ApproachDistance))
                        HunterStatus = HunterMode.Idle;

                    break;
                case HunterMode.Persue:
                    if (!InRange(PersueDistance))
                        HunterStatus = HunterMode.Approach;
                    break;

            }
        }

        private bool InRange(float Range)
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            return distance <= Range;
        }
        private void UpdateHunterBehavoir()
        {
            List<IBehavor> behavors = GetBehavoirs(m_HunterBehavoirs[HunterStatus]);
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