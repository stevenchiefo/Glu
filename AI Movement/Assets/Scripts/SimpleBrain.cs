using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [RequireComponent(typeof(Steering))]
    public class SimpleBrain : MonoBehaviour
    {
        public enum BehaviorEnum
        {
            Keyboard,
            SeekClickPoint,
            Seek,
            Pursue,
            Evade,
            Wander,
            FollowPath,
            Hide,
            NotSet,
        };

        [Header("Manual")]
        public BehaviorEnum m_Behavior;
        public GameObject m_Target;

        [Header("Private")]
        private Steering m_Steering;

        public SimpleBrain()
        {
            m_Behavior = BehaviorEnum.NotSet;
            m_Target = null;
        }

        private void Start()
        {
            if (m_Behavior == BehaviorEnum.Keyboard || m_Behavior == BehaviorEnum.SeekClickPoint)
                m_Target = null;
            else
            {
                if (m_Target == null)
                    m_Target = GameObject.Find("Player");
                if (m_Target == null)
                    m_Target = GameObject.Find("Target");
            }

            m_Steering = GetComponent<Steering>();

            List<IBehavor> behavors = new List<IBehavor>();
            string label = m_Behavior.ToString();
            switch (m_Behavior)
            {
                case BehaviorEnum.Keyboard:
                    behavors.Add(new KeyBoard());
                    break;

                case BehaviorEnum.SeekClickPoint:
                    behavors.Add(new ClickSeekPoint());
                    break;
                case BehaviorEnum.Seek:
                    behavors.Add(new Seek());
                    break;

                default:
                    Debug.LogError($"Behavior of Type{m_Behavior} not implemented yet!");
                    break;

                    
            }
            m_Steering.SetBehaviors(behavors, label);
        }
    }
}
