using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Fleer : MonoBehaviour
{
    private Transform m_player;
    public NavMeshAgent m_agent;
    public Pose m_pose;

    private void Start()
    {
        m_player = GameManager.Instance.player.transform;
        m_agent = GetComponent<NavMeshAgent>();

        RunFrom();

        m_pose = new Pose(transform.position, transform.rotation);

        m_areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
    }

    enum FleeState
    {
        Idle,
        Fleeing,
        Returning
    }

    [SerializeField]
    private FleeState state = FleeState.Idle;

    private void Update()
    {
        if (m_agent.hasPath && m_agent.pathStatus != NavMeshPathStatus.PathComplete) return;

        if (Vector3.Distance(m_player.position, transform.position) <= m_fleeDistance && m_agent.remainingDistance >= m_agent.stoppingDistance)
        {
            RunFrom();
            return;
        }

        if (m_agent.isPathStale || m_agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (Vector3.Distance(m_player.position, m_pose.position) >= m_fleeDistance)
            {
                m_agent.SetDestination(m_pose.position);
                return;
            }
        }

        // switch (state)
        // {
        //     case FleeState.Idle:
        //         if (Vector3.Distance(m_player.position, transform.position) <= m_fleeDistance)
        //         {
        //             RunFrom();
        //         }
        //
        //         break;
        //     case FleeState.Fleeing:
        //     //     if (Vector3.Distance(m_player.position, m_pose.position) <= m_fleeDistance)
        //     //     {
        //     //         m_agent.SetDestination(m_pose.position);
        //     //     }
        //     //
        //     //     break;
        //     // case FleeState.Returning:
        //     //     if (Vector3.Distance(m_player.position, m_pose.position) <= .2f)
        //     //     {
        //     //         state = FleeState.Idle;
        //     //     }
        //
        //         break;
        // }


        // if (Time.time > nextTurnTime)
    }

    private NavMeshHit m_hit;
    private int m_areaMask;

    private void RunFrom()
    {
        NavMesh.SamplePosition(GetFleePoint(), out m_hit, 5, m_areaMask);

        Debug.Log(m_hit.position);
        m_agent.SetDestination(m_hit.position);

        state = FleeState.Fleeing;
    }

    public float m_maxRadiusFlee = 30;
    public float m_fleeDistance = 10;
    public int m_count = 10;
    public SerializedDictionary<Vector3, float> posAndNote = new();

    private Vector3 GetFleePoint()
    {
        posAndNote.Clear();
        for (int i = 0; i < m_count; i++)
        {
            Vector3 point = Vector3.zero;
            int step = 360 / m_count;
            point.x = transform.position.x + m_maxRadiusFlee * math.cos((step * i) * math.PI / 180);
            point.z = transform.position.z + m_maxRadiusFlee * math.sin((step * i) * math.PI / 180);
            if (NavMesh.SamplePosition(point, out NavMeshHit havHit, m_maxRadiusFlee / 2, NavMesh.AllAreas))
            {
                point = havHit.position;
            }

            NavMeshPath path = new();
            if (NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path))
            {
                Vector3 last = path.corners.Last();
                float distance = Vector3.Distance(path.corners.Last(), m_player.position);
                posAndNote.TryAdd(last, distance);
            }
        }

        return posAndNote.OrderByDescending(x => x.Value).First().Key;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_agent == null) return;
        Handles.Label(transform.position + Vector3.up,
            @$"status: {m_agent.pathStatus}
remaining dst {m_agent.remainingDistance}
isPathStale {m_agent.isPathStale}");
    }

    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawCube(m_hit.position, Vector3.one * .2f);
    //
    //
    //     // max = float.MinValue;
    //     // min = float.MaxValue;
    //
    //     Dictionary<Vector3, float> posAndNote;
    //     posAndNote = new Dictionary<Vector3, float>();
    //
    //     for (int i = 0; i < m_count; i++)
    //     {
    //         Vector3 point = Vector3.zero;
    //         int step = 360 / m_count;
    //         point.x = transform.position.x + m_radiusFlee * math.cos((step * i) * math.PI / 180);
    //         point.z = transform.position.z + m_radiusFlee * math.sin((step * i) * math.PI / 180);
    //
    //
    //         NavMeshHit hit;
    //         if (NavMesh.SamplePosition(point, out hit, m_radiusFlee / 2, NavMesh.AllAreas))
    //         {
    //             Gizmos.DrawCube(hit.position, Vector3.one * 1);
    //             point = hit.position;
    //         }
    //
    //         // Gizmos.DrawSphere(point, .1f);
    //         NavMeshPath path = new();
    //         bool success = NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path);
    //         if (success)
    //         {
    //             // Vector3 first = path.corners.First();
    //             Vector3 last = path.corners.Last();
    //
    //
    //             float distance = Vector3.Distance(path.corners.Last(), m_player.position);
    //
    //             posAndNote.Add(last, distance);
    //
    //             // if (distance < min)
    //             // {
    //             //     min = distance;
    //             // }
    //             //
    //             // if (distance > max)
    //             // {
    //             //     max = distance;
    //             // }
    //             //
    //             // float remapped = math.remap(min, max, 0, 1, distance);
    //             //
    //             // Gizmos.color = Color.Lerp(Color.red, Color.green, remapped);
    //             //
    //             // Gizmos.DrawLine(transform.position, first);
    //             // for (int u = 0; u < path.corners.Length - 1; u++)
    //             // {
    //             //     Gizmos.DrawLine(path.corners[u], path.corners[u + 1]);
    //             // }
    //             //
    //             // Gizmos.DrawCube(last, Vector3.one * math.lerp(.2f, 1, remapped));
    //             // Handles.Label(last, path.status.ToString());
    //             // Gizmos.color = Color.white;
    //         }
    //     }
    // }
}