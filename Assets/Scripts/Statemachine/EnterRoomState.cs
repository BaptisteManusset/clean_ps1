using UnityEngine;

public class EnterRoomState : SimpleState
{
    [SerializeField] private Room room;

    // public override void Enter()
    // {
    //     room.OnEntered += OnEntered;
    //     room.OnExisted += OnExisted;
    // }
    //
    // public override void Exit()
    // {
    //     room.OnEntered -= OnEntered;
    //     room.OnExisted -= OnExisted;
    // }

    private void OnEntered(Room a_room)
    {
        Debug.Log($"Player Entered {a_room.gameObject.name}");
    }

    private void OnExisted(Room a_room)
    {
        Debug.Log($"Player Existed {a_room.gameObject.name}");
    }
}