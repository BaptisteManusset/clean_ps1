using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    public ItemData key;

    public Transform anchor;

    public Door Destination;

    public NavMeshLink MeshLink;

    private void Awake()
    {
        MeshLink = GetComponent<NavMeshLink>();

        MeshLink.endTransform = Destination.anchor;
    }

    public void Use()
    {
        if(countdown) return;
        if (Destination == null) return;
        if ((key == null || !Inventory.Instance.Contains(key)) && key != null) return;

        Destination.GoTo();
    }

    public void ExitUse()
    {
    }

    private void GoTo()
    {
        Player.Instance.Teleport(anchor);
        StartCoroutine(PlayingCountdown());
    }

    public bool countdown = false;

    IEnumerator PlayingCountdown()
    {
        countdown = true;
        yield return new WaitForSecondsRealtime(1f);
        countdown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Destination ? Color.green : Color.red;

        Gizmos.DrawCube(anchor.position, Vector3.one * .2f);
        Gizmos.DrawCube(anchor.position + anchor.forward, Vector3.one * .1f);
        Gizmos.DrawLine(anchor.position, anchor.position + anchor.forward);
    }

    private void OnDrawGizmosSelected()
    {
        if (Destination == null) return;
        Gizmos.color = new Color(0.31f, 0.85f, 1f);
        Gizmos.DrawLine(transform.position + Vector3.up, Destination.transform.position);
    }

    [ContextMenu("Link doors")]
    private void LinkDoors()
    {
        List<Door> doors = new();

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            if (gameObject.TryGetComponent(out Door door))
            {
                doors.Add(door);
            }
        }

        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].SetOtherDoor(doors[(i + 1) % doors.Count]);
            EditorUtility.SetDirty(doors[i]);
        }
    }

    private void SetOtherDoor(Door a_door)
    {
        Destination = a_door;
        if (MeshLink == null) MeshLink = GetComponent<NavMeshLink>();
        MeshLink.endTransform = Destination.anchor;
    }
}