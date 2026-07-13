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
            door.OnUse += OnUse;
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
            door.OnUse -= OnUse;
        }
    }


    private void OnUse(Door.DoorUseState doorUseState)
    {
        Release();
        m_stateMachine.NextState();
    }
}