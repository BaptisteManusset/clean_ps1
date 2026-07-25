using System;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour, IRequestRoomAwaker
{
    private VisibleEvent m_visibleEvent;
    private NavMeshAgent m_navMeshAgent;

    public bool isVisible = false;
    public float maxWarpDistance = 5;
    public float maxRadius = 30;
    public Transform target;

    private void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_visibleEvent = GetComponent<VisibleEvent>();
        SendToSleep();
    }


    private void UpdateLogic()
    {
        Transform angel = transform;

        Vector3 forward = target.TransformDirection(Vector3.forward);
        Vector3 toOther = Vector3.Normalize(angel.position - target.position);


        bool isBehind = Vector3.Dot(forward, toOther) < 0;


        if (isBehind)
        {
            if (isVisible)
            {
                isVisible = false;
                Debug.Log("become hidden");
                Warp();
                return;
            }

            Debug.Log("The other transform is behind me!");
        }
        else
        {
            if (!isVisible)
            {
                isVisible = true;
                Debug.Log("become visible");
            }
        }
    }

    private void Update()
    {
        UpdateLogic();
    }


    private void Warp()
    {
        Vector3 playerPos = target.position;
        Vector3 diff = transform.position - playerPos;
        diff = Vector3.ClampMagnitude(diff, maxWarpDistance);
        // transform.position -= diff;
        m_navMeshAgent.Warp(transform.position - diff);
        // Debug.Log("Warped");
    }

    public void WakeUp()
    {
        enabled = true;
    }

    public void SendToSleep()
    {
        enabled = false;
    }
}