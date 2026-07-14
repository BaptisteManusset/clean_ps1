using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
[SelectionBase]
public class Room : MonoBehaviour
{
    [SerializeField] private List<Door> Doors = new();

    [SerializeField] private Bounds m_bounds = new(Vector3.zero, Vector3.zero);
    public event Action<Room> OnEntered;
    public event Action<Room> OnExisted;

    private void Awake()
    {
        foreach (Door door in Doors)
        {
            door.OnExitedDoor += EnterRoom;
        }

        foreach (Door door in Doors)
        {
            door.OnUseDoor += ExistRoom;
        }
    }

    private void OnDestroy()
    {
        foreach (Door door in Doors)
        {
            door.OnExitedDoor -= EnterRoom;
        }

        foreach (Door door in Doors)
        {
            door.OnUseDoor -= ExistRoom;
        }
    }

    private void ExistRoom(Door.DoorUseState obj)
    {
        OnExisted?.Invoke(this);
    }

    private void EnterRoom()
    {
        OnEntered?.Invoke(this);
    }

    private void Reset()
    {
        ListDoors();
        CalculateBounds();
    }

    private void ListDoors()
    {
        Doors = GetComponentsInChildren<Door>(true).ToList();
    }

    private void CalculateBounds()
    {
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        m_bounds = colliders.First().bounds;

        foreach (Collider collider in colliders)
        {
            m_bounds.Encapsulate(collider.bounds);
        }

        m_bounds.center -= transform.position;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (m_bounds.size == Vector3.zero || m_bounds.center == Vector3.zero)
        {
            CalculateBounds();
            EditorUtility.SetDirty(this);
        }

        if (Doors.Count == 0)
        {
            ListDoors();
            EditorUtility.SetDirty(this);

            SetDoorsNames();
        }

        Gizmos.color = new Color(0.67f, 0.68f, 1f);
        Gizmos.DrawWireCube(m_bounds.center + transform.localPosition, m_bounds.size);
        Handles.Label(m_bounds.center + transform.localPosition, gameObject.name, EditorStyles.boldLabel);
    }

    [EditorButton]
    public void SetDoorsNames()
    {
        if (Doors.Count == 0)
        {
            ListDoors();
            EditorUtility.SetDirty(this);
        }

        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].gameObject.name = $"Door {gameObject.name} {i}";
            EditorUtility.SetDirty(Doors[i].gameObject);
        }
    }
#endif
}