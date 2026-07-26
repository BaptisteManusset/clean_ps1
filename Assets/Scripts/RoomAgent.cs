using System;
using UnityEngine;

public class RoomAgent : MonoBehaviour
{
    private const float SCAN_RADIUS = 1;
    public Room currentRoom;

    public event Action<Room> CurrentRoomChanged;

    private void Awake()
    {
        TryGetCurrentRoom();
    }

    public void TryGetCurrentRoom()
    {
        Collider[] results = new Collider[6];
        int size = Physics.OverlapSphereNonAlloc(transform.position, SCAN_RADIUS, results);
        if (size <= 0)
        {
            Debug.LogWarning("no room found");
            return;
        }

        foreach (Collider result in results)
        {
            if (result == null) continue;
            Room room = result.GetComponentInParent<Room>();
            if (!room) continue;
            currentRoom = room;
            CurrentRoomChanged?.Invoke(currentRoom);
            break;
        }
    }
}