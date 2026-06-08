using System;
using System.Collections;
using System.Collections.Generic;
using JSAM;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    public ItemData key;

    public Transform anchor;

    public Door Destination;

    public NavMeshLink MeshLink;

    public SoundFileObject openSound;
    public SoundFileObject lockSound;

    private void Awake()
    {
        if (Destination == null) return;
        MeshLink = GetComponent<NavMeshLink>();

        MeshLink.endTransform = Destination.anchor;
    }

    public void Use()
    {
        if (countdown) return;
        if (Destination == null)
        {
            CenterMessage.Instance.PublishMessage("La porte est bloquée");
            if (lockSound) lockSound.Play(transform.position);
            return;
        }

        if ((key == null || !Inventory.Instance.Contains(key)) && key != null)
        {
            CenterMessage.Instance.PublishMessage("Une clé est nécessaire");
            if (lockSound) lockSound.Play(transform.position);
            return;
        }

        Destination.GoTo();
        if (openSound) openSound.Play(transform.position);
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
        if (Destination == null)
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
        else
        {
            Gizmos.DrawCube(anchor.position, Vector3.one * .2f);
            Gizmos.DrawCube(anchor.position + anchor.forward, Vector3.one * .1f);
            Gizmos.DrawLine(anchor.position, anchor.position + anchor.forward);
            Gizmos.color = new Color(0.31f, 0.85f, 1f);
            Gizmos.DrawLine(transform.position + Vector3.up, Destination.transform.position);
        }
    }


    [MenuItem("CONTEXT/MeshFilter/Custom Mesh Filter Item")]
    static void TestMeshFilterMenuItem()
    {
        Debug.Log("MeshFilter Menu Item");
        LinkDoors();
    }


    [ContextMenu("Link doors")]
    private static void LinkDoors()
    {
        List<Door> doors = new();

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Door door = gameObject.GetComponentInChildren<Door>(true);
            if (door)
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