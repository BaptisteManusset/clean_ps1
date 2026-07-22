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

    public float maxWarpDistance = 5f;

    private void Warp()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        if (Vector3.Distance(playerPos, transform.position) < maxWarpDistance) return;
        Vector3 diff = transform.position - playerPos;
        diff = Vector3.ClampMagnitude(diff, maxWarpDistance);
        m_agent.Warp(diff);
        Debug.Log("Warped");
    }
}