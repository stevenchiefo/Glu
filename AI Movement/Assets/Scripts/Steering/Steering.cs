using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{

    using BehavorList = List<IBehavor>;
    [RequireComponent(typeof(Rigidbody))]
    public class Steering : MonoBehaviour
    {
        public string m_Label;
        public SteeringSettings m_Settings;

        public Vector3 m_Position;
        public Vector3 m_Velocity;
        public Vector3 m_Steering;
        private Rigidbody m_Rb;

        private BehavorList m_BehavorList = new BehavorList();
        private ObjectAvoidance m_ObjectAvoidance;



        // Start is called before the first frame update
        void Start()
        {
            m_Position = transform.position;
            m_Rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            m_Steering = Vector3.zero;
            CheckPriortys();
            foreach (Behavor behavor in m_BehavorList)
            {
                m_Steering += behavor.CaculateSteeringForce(Time.deltaTime, new BehavorContext(m_Position, m_Velocity, m_Settings)) * behavor.Priorty;
                Debug.DrawLine(m_Position, m_Position + m_Steering);
            }

            m_Steering.y = 0f;

            m_Steering = Vector3.ClampMagnitude(m_Steering, m_Settings.m_MaxSteeringForce);
            m_Steering /= m_Settings.m_Mass;

            m_Velocity = Vector3.ClampMagnitude(m_Velocity + m_Steering, m_Settings.m_MaxSpeed);
            //m_Position = m_Position + m_Velocity * Time.fixedDeltaTime;

            m_Rb.MovePosition(transform.position + m_Velocity * Time.fixedDeltaTime);
            m_Position = m_Rb.position;
            transform.LookAt(m_Position + Time.fixedDeltaTime * m_Velocity);

        }
        private void OnDrawGizmos()
        {
            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.OnDrawGizmos(new BehavorContext(m_Position, m_Velocity, m_Settings));
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
                    if (behavor.Label != m_ObjectAvoidance.Label)
                        behavor.SetPriorty(OtherPriortys);
                }
            }
        }

        public void SetBehaviors(ObjectAvoidance objectAvoidance, BehavorList behavorList, string label = "")
        {

            m_Label = label;
            m_BehavorList = behavorList;
            m_ObjectAvoidance = objectAvoidance;

            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.Start(new BehavorContext(m_Position, m_Velocity, m_Settings));
            }
        }




    }
}
