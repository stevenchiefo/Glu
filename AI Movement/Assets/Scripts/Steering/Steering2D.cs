using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    using BehavorList = List<IBehavor>;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Steering2D : MonoBehaviour
    {
        public string m_Label;
        public SteeringSettings m_Settings;

        public Vector3 m_Position;
        public Vector3 m_Velocity;
        public Vector3 m_Steering;
        private Rigidbody2D m_Rb;

        private BehavorList m_BehavorList = new BehavorList();
        private ObjectAvoidance m_ObjectAvoidance;
        private Arrive m_Arrive;

        private float m_Speed;
        private bool m_UseSpeed;
        // Start is called before the first frame update
        private void Start()
        {
            m_Position = transform.position;
            m_Rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_Steering = Vector3.zero;
            CheckPriortys();
            CheckForArrive();
            MoveObject();
            
        }

        public void SetSpeed(float _speed)
        {
            m_UseSpeed = true;
            m_Speed = _speed;
        }

        private void MoveObject()
        {
            foreach (Behavor behavor in m_BehavorList)
            {
                m_Steering += behavor.CaculateSteeringForce(Time.deltaTime, new BehavorContext(m_Position, m_Velocity, m_Settings)) * behavor.Priorty;
            }
            m_Steering.z = 0f;

            m_Steering = Vector3.ClampMagnitude(m_Steering, m_Settings.m_MaxSteeringForce);
            m_Steering /= m_Settings.m_Mass;

            if (!m_UseSpeed)
            {
                m_Velocity = Vector3.ClampMagnitude(m_Velocity + m_Steering, m_Settings.m_MaxSpeed);
            }
            else
            {
                m_Velocity = Vector3.ClampMagnitude(m_Velocity + m_Steering, m_Speed);
            }

            m_Rb.MovePosition(transform.position + m_Velocity * Time.fixedDeltaTime);
            m_Position = m_Rb.position;
        }

        private void OnDrawGizmos()
        {
            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.OnDrawGizmos(new BehavorContext(m_Position, m_Velocity, m_Settings));
            }
        }

        private void CheckForArrive()
        {
            if (m_Arrive != null)
            {
                if (m_Arrive.DoBrake)
                {
                    foreach (IBehavor behavor in m_BehavorList)
                    {
                        if (behavor.Label != m_Arrive.Label)
                        {
                            if (m_ObjectAvoidance != null)
                            {
                                if (behavor.Label != m_ObjectAvoidance.Label)
                                {
                                    behavor.SetPriorty(0.0f);
                                }
                            }
                            else
                            {
                                behavor.SetPriorty(0.0f);
                            }
                        }
                    }
                    m_Arrive.SetPriorty(1);
                }
            }
        }

        private void CheckPriortys()
        {
            if (m_ObjectAvoidance != null)
            {
                float Prioty = m_ObjectAvoidance.CaculatePriorty(new BehavorContext(m_Position, m_Velocity, m_Settings));
                float OtherPriortys = 1f - Prioty;
                foreach (IBehavor behavor in m_BehavorList)
                {
                    if (m_Arrive != null)
                    {
                        if (behavor.Label == m_Arrive.Label)
                        {
                            behavor.SetPriorty(0);
                            continue;
                        }
                    }

                    if (behavor.Label != m_ObjectAvoidance.Label)
                        behavor.SetPriorty(OtherPriortys);
                }
            }
        }

        public void SetSteeringSettings(SteeringSettings steeringSettings)
        {
            m_Settings = steeringSettings;
        }

        public void SetBehaviors(ObjectAvoidance objectAvoidance, Arrive arrive, BehavorList behavorList, string label = "")
        {
            m_Label = label;
            m_BehavorList = behavorList;
            m_ObjectAvoidance = objectAvoidance;
            m_Arrive = arrive;

            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.Start(new BehavorContext(m_Position, m_Velocity, m_Settings));
                if (m_Arrive != null && m_ObjectAvoidance != null)
                {
                    if (behavor.Label != m_Arrive.Label && behavor.Label != m_ObjectAvoidance.Label)
                    {
                        behavor.SetPriorty(1);
                    }
                }
                else
                {
                    behavor.SetPriorty(1);
                }
            }
        }
    }
}