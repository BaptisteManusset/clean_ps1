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
    [SerializeField] public List<Door> Doors = new();
    [SerializeField] public List<Light> Lights = new();


    [SerializeField] private Bounds m_bounds = new(Vector3.zero, Vector3.zero);
    // public event Action<Room> OnEntered;
    // public event Action<Room> OnExisted;

    private void Awake()
    {
        foreach (Door door in Doors)
        {
            door.OnUseDoor += PlayerExitPreviousRoom;
        }

        DisableLights();
    }

    private void DisableLights()
    {
        foreach (Light l in Lights)
        {
            l.gameObject.SetActive(false);
        }
    }

    private void EnableLights()
    {
        foreach (Light l in Lights)
        {
            l.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach (Door door in Doors)
        {
            door.OnUseDoor -= PlayerExitPreviousRoom;
        }
    }

    public void PlayerEnterNewRoom()
    {
        EnableLights();
    }

    private void PlayerExitPreviousRoom(Door.DoorUseData doorUseData)
    {
        if (doorUseData.State != Door.DoorUseState.Success) return;

        doorUseData.originRoom?.DisableLights();
        doorUseData.destinationRoom.EnableLights();
    }

#if UNITY_EDITOR


    private void ListDoors()
    {
        if (Doors.Count != 0) return;
        Doors = GetComponentsInChildren<Door>(true).ToList();
        EditorUtility.SetDirty(this);
    }

    private void ListLights()
    {
        if (Lights.Count != 0) return;
        Lights = GetComponentsInChildren<Light>(true).ToList();
        EditorUtility.SetDirty(this);
    }

    [EditorButton]
    private void Reset()
    {
        ForceUpdateData();
    }

    [ContextMenu("Force Update Data")]
    private void ForceUpdateData()
    {
        ListLights();
        ListDoors();
        CalculateBounds();
        SetDoorsNames();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.67f, 0.68f, 1f);
        Gizmos.DrawWireCube(m_bounds.center + transform.localPosition, m_bounds.size);
        Handles.Label(m_bounds.center + transform.localPosition, gameObject.name, EditorStyles.boldLabel);
    }

    public void SetDoorsNames()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].gameObject.name = $"Door {gameObject.name} {i}";
            EditorUtility.SetDirty(Doors[i].gameObject);
        }
    }

    private void CalculateBounds()
    {
        if (m_bounds.size != Vector3.zero && m_bounds.center != Vector3.zero) return;

        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        m_bounds = colliders.First().bounds;

        foreach (Collider collider in colliders)
        {
            m_bounds.Encapsulate(collider.bounds);
        }

        m_bounds.center -= transform.position;
        EditorUtility.SetDirty(this);
    }
#endif
}