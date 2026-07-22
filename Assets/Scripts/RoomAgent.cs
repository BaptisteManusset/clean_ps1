using System;
using UnityEngine;

public class RoomAgent : MonoBehaviour
{
    public Room currentRoom;

    private void Awake()
    {
        RequestCurrentRoom();
    }

    private void RequestCurrentRoom()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider result in results)
        {
            Room room = result.GetComponentInParent<Room>();
            if (room != null)
            {
                currentRoom = room;
            }
        }
    }
}