using System;
using UnityEngine;
using UnityEngine.AI;


public class AgentLinkMover : MonoBehaviour
{
    public NavMeshAgent agent;
    
    public OffMeshLinkData linkData => agent.currentOffMeshLinkData;

    [SerializeField]
    public bool isOffLink = false;

    public event Action OnEnterMeshLink;
    public event Action OnExitMeshLink;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
    }

    private void OnEnable()
    {
        isOffLink = false;
    }

    private void Update()
    {
        if (agent.isOnOffMeshLink && !isOffLink)
        {
            isOffLink = true;
            OnEnterMeshLink?.Invoke();
        }

        if (!agent.isOnOffMeshLink && isOffLink)
        {
            isOffLink = false;
            OnExitMeshLink?.Invoke();
        }
    }
}