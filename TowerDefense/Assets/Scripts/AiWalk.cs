using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiWalk : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private IEnemy m_Enemy;

    private void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Enemy = GetComponent<IEnemy>();
    }

    private void Update()
    {
        m_NavMeshAgent.SetDestination(Finish.Instance.gameObject.transform.position);
    }

    

}
