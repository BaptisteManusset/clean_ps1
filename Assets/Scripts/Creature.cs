using System;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
    NavMeshAgent m_agent;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_agent.SetDestination(GameManager.Instance.player.transform.position);
    }

    private void Update()
    {
        if (m_agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            m_agent.SetDestination(GameManager.Instance.player.transform.position);
        }
    }
}