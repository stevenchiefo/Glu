using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{

    using BehavorList = List<IBehavor>;
    public class Steering : MonoBehaviour
    {
        public string m_Label;
        public SteeringSettings m_Settings;

        public Vector3 m_Position;
        public Vector3 m_Velocity;
        public Vector3 m_Steering;

        private BehavorList m_BehavorList = new BehavorList();



        // Start is called before the first frame update
        void Start()
        {
            m_Position = transform.position;

        }
        private void FixedUpdate()
        {
            m_Steering = Vector3.zero;
            foreach (IBehavor behavor in m_BehavorList)
            {
                m_Steering = behavor.CaculateSteeringForce(Time.deltaTime, new BehavorContext(m_Position, m_Velocity, m_Settings));
            }

            m_Steering.y = 0f;

            m_Steering = Vector3.ClampMagnitude(m_Steering, m_Settings.m_MaxSteeringForce);
            m_Steering /= m_Settings.m_Mass;

            m_Velocity = Vector3.ClampMagnitude(m_Velocity + m_Steering, m_Settings.m_MaxSpeed);
            m_Position = m_Position + m_Velocity * Time.fixedDeltaTime;

            transform.position = m_Position;
            transform.LookAt(m_Position + Time.fixedDeltaTime * m_Velocity);

        }
        private void OnDrawGizmos()
        {
            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.OnDrawGizmos(new BehavorContext(m_Position, m_Velocity, m_Settings));
            }
        }

        public void SetBehaviors(BehavorList behavorList, string label = "")
        {
            m_Label = label;
            m_BehavorList = behavorList;

            foreach (IBehavor behavor in m_BehavorList)
            {
                behavor.Start(new BehavorContext(m_Position, m_Velocity, m_Settings));
            }
        }

        

        
    }
}
