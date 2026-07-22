using System;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{
    private VisibleEvent m_visibleEvent;
    private NavMeshAgent m_agent;

    public bool isVisible = false;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_visibleEvent = GetComponent<VisibleEvent>();
    }

    private void Update()
    {
        float dot = Vector3.Dot(GameManager.Instance.Cam.transform.forward,
            transform.InverseTransformPoint(GameManager.Instance.Cam.transform.position));
        Debug.Log($"{dot:F}");


        Vector3 directionToTarget = GameManager.Instance.Cam.transform.position - transform.position;
        float angle = Vector3.Angle(GameManager.Instance.Cam.transform.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        if (Mathf.Abs(angle) < 90 && distance < 100)
        {
            if (isVisible)
            {
                isVisible = false;
                Warp();
            }
        }
        else
        {
            if (!isVisible)
            {
                isVisible = true;
            }
        }
    }


    // private void OnBecameVisible()
    // {
    //     isVisible = false;
    //     Debug.Log("OnBecameVisible");
    // }
    //
    // private void OnBecameInvisible()
    // {
    //     isVisible = true;
    // }

    private void Warp()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        // if (Vector3.Distance(playerPos, transform.position) >= m_agent.radius * 1.5f)
        // {
            m_agent.Warp(Vector3.Lerp(transform.position, playerPos, .1f));
            Debug.Log("Warped");
            Debug.Log("OnBecameInvisible");
        // }
    }
}