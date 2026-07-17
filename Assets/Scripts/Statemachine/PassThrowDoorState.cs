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

        TodoListUI.Instance.SetText(textToDisplay);
    }

    public override void Exit()
    {
        Release();
        base.Exit();
    }

    private void Release()
    {
        foreach (Door door in Doors)
        {
            door.OnUseDoor -= OnUseDoor;
        }
    }


    private void OnUseDoor(Door.DoorUseState doorUseState)
    {
        Release();
        m_stateMachine.NextState();
        
        
    }
}