using UnityEngine;

public class PassThrowDoorState : SimpleState
{
    [SerializeField]
    private string textToDisplay = "Dirigez vous vers le batiment";

    [SerializeField]
    private Door[] Doors;


    public override void Enter()
    {
        foreach (Door door in Doors)
        {
            door.OnUseDoor += OnUseDoor;
        }

        base.Enter();

        if (TodoListUI.Instance) TodoListUI.Instance.SetText(textToDisplay);
    }

    public override void Exit()
    {
        Unsubscribe();
        base.Exit();
    }

    private void Unsubscribe()
    {
        foreach (Door door in Doors)
        {
            door.OnUseDoor -= OnUseDoor;
        }
    }


    private void OnUseDoor(Door.DoorUseData doorUseData)
    {
        if (doorUseData.State != Door.DoorUseState.Success) return;
        Unsubscribe();
        m_stateMachine.NextState();
    }
}