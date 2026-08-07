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
    private RoomCulling Culling;
    [SerializeField] public List<Door> Doors = new();


    // [SerializeField] private Bounds m_bounds = new(Vector3.zero, Vector3.zero);

    private void Awake()
    {
        Culling = GetComponent<RoomCulling>();
        foreach (Door door in Doors)
        {
            if (door == null)
            {
                Debug.LogWarning("Missing Door inside " + gameObject.name, gameObject);
                continue;
            }

            door.OnUseDoor += PlayerExitPreviousRoom;
        }
    }


    private void OnDestroy()
    {
        foreach (Door door in Doors)
        {
            if (door == null)
            {
                continue;
            }

            door.OnUseDoor -= PlayerExitPreviousRoom;
        }
    }

    private void PlayerExitPreviousRoom(Door.DoorUseData doorUseData)
    {
        if (doorUseData.State != Door.DoorUseState.Success) return;

        doorUseData.originRoom?.Culling.DisableLights();
        doorUseData.destinationRoom.Culling.EnableLights();
    }

#if UNITY_EDITOR


    private void ListDoors()
    {
        if (Doors.Count != 0) return;
        Doors = GetComponentsInChildren<Door>(true).ToList();
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
        ListDoors();
        // CalculateBounds();
        SetDoorsNames();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        Gizmos.color = Handles.color = new Color(0.67f, 0.68f, 1f);
        // Gizmos.DrawWireCube(Vector3.zero + m_bounds.center, m_bounds.size);
        Handles.Label(Vector3.zero, gameObject.name, EditorStyles.boldLabel);
    }

    public void SetDoorsNames()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].gameObject.name = $"Door {gameObject.name} {i}";
            EditorUtility.SetDirty(Doors[i].gameObject);
        }
    }

    // private void CalculateBounds()
    // {
    //     if (m_bounds.size != Vector3.zero && m_bounds.center != Vector3.zero) return;
    //
    //     Collider[] colliders = transform.GetComponentsInChildren<Collider>();
    //
    //     m_bounds = colliders.First().bounds;
    //
    //     foreach (Collider collider in colliders)
    //     {
    //         m_bounds.Encapsulate(collider.bounds);
    //     }
    //
    //     m_bounds.center -= transform.position;
    //     EditorUtility.SetDirty(this);
    // }
#endif
}