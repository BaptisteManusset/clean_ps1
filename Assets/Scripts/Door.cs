using System;
using System.Collections;
using System.Collections.Generic;
using JSAM;
using Unity.AI.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[SelectionBase]
public class Door : MonoBehaviour, IUsable
{
    public enum DoorUseState
    {
        Fail,
        Locked,
        Success
    }

    public ItemData key;

    public Transform anchor;

    // public Door Destination;
    public DestinationGetter Getter;

    public NavMeshLink MeshLink;

    public SoundFileObject openSound;
    public SoundFileObject lockSound;

    public bool countdown = false;

    private void Awake()
    {
        if (Getter == null) return;
        MeshLink = GetComponent<NavMeshLink>();
    }

    private void Start()
    {
        Door door = Getter.Get();
        if (door) MeshLink.endTransform = door.anchor;
    }

    public void Use()
    {
        if (countdown)
        {
            OnUse?.Invoke(DoorUseState.Fail);
            return;
        }

        if (Getter.Get() == null)
        {
            CenterMessage.Instance.PublishMessage("La porte est bloquée");
            if (lockSound) lockSound.Play(transform.position);
            OnUse?.Invoke(DoorUseState.Locked);
            return;
        }

        if ((key == null || !Inventory.Instance.Contains(key)) && key != null)
        {
            CenterMessage.Instance.PublishMessage("Une clé est nécessaire");
            if (lockSound) lockSound.Play(transform.position);
            OnUse?.Invoke(DoorUseState.Locked);
            return;
        }

        Getter.Get().GoTo();
        if (openSound) openSound.Play(transform.position);
        OnUse?.Invoke(DoorUseState.Success);
    }

    public void ExitUse()
    {
    }

    public event Action<DoorUseState> OnUse;


    private void GoTo()
    {
        Player.Instance.Teleport(Getter.Get().anchor);
        StartCoroutine(PlayingCountdown());
    }

    IEnumerator PlayingCountdown()
    {
        countdown = true;
        yield return new WaitForSecondsRealtime(1f);
        countdown = false;
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(anchor.position, Vector3.one * .2f);
        Gizmos.DrawCube(anchor.position + anchor.forward, Vector3.one * .1f);
        Gizmos.DrawLine(anchor.position, anchor.position + anchor.forward);
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

    public void SetOtherDoor(Door a_door)
    {
        Getter.Destination = a_door;
        if (MeshLink == null) MeshLink = GetComponent<NavMeshLink>();
        MeshLink.endTransform = Getter.Destination.anchor;
    }
#endif
}