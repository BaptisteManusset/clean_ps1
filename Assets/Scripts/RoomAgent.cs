using System;
using UnityEngine;

public class RoomAgent : MonoBehaviour
{
    public Room currentRoom;

    private IRequestRoomAwaker[] m_elementsToToggle;

    private void Awake()
    {
        m_elementsToToggle = GetComponentsInChildren<IRequestRoomAwaker>();

        RequestCurrentRoom();
    }

    private void OnEnable()
    {
        
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


        UpdateElements();
    }

    private void UpdateElements()
    {
        if (currentRoom == GameManager.Instance.player.CurrentRoom)
        {
            foreach (IRequestRoomAwaker roomAwaker in m_elementsToToggle)
            {
                roomAwaker.WakeUp();
            }
        }
        else
        {
            foreach (IRequestRoomAwaker roomAwaker in m_elementsToToggle)
            {
                roomAwaker.SendToSleep();
            }
        }
    }
}