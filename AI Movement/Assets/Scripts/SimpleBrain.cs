﻿using System.Collections;
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
        public BehaviorEnum m_Behavior;

        public GameObject m_Target;

        [Header("Private")]
        private Steering m_Steering;

        [Header("WaitPoints")]
        public List<Transform> m_WaitPoints;

        [Header("Object Avoidance")]
        public LayerMask LayerMask;

        public bool m_ObjectAvoidanceActive;
        public float m_Radius;

        [Header("Arrive")]
        public bool m_ArriveActive;

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

                case BehaviorEnum.Hide:
                    behavors.Add(new Hide(m_Target.transform));
                    break;

                default:
                    Debug.LogError($"Behavior of Type{m_Behavior} not implemented yet!");
                    break;
            }
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
            behavors[0].Label = label;
            m_Steering.SetBehaviors(objectAvoidance, arrive, behavors, label);
        }
    }
}