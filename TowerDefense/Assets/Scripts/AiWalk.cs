using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiWalk : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private IEnemy m_Enemy;
    private bool m_Walk;

    private void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Enemy = GetComponent<IEnemy>();
        m_Walk = false;
    }

    private void Update()
    {
        if (m_Walk)
        {

            m_NavMeshAgent.SetDestination(Finish.Instance.gameObject.transform.position);
        }
    }

    public void SetWalk(bool boolean)
    {
        m_Walk = boolean;
    }



}
