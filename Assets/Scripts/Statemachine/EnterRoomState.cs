using UnityEngine;

public class EnterRoomState : SimpleState
{
    [SerializeField] private Room room;

    public override void Enter()
    {
        room.OnEntered += OnEntered;
        room.OnExisted += OnExisted;
    }

    public override void Exit()
    {
        room.OnEntered -= OnEntered;
        room.OnExisted -= OnExisted;
    }

    private void OnEntered(Room room1)
    {
        Debug.Log("Player Entered");
    }

    private void OnExisted(Room room1)
    {
        Debug.Log("Player Existed");
    }
}