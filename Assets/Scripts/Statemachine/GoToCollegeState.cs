using UnityEngine;

public class GoToCollegeState : SimpleState
{
    [SerializeField]
    private Door[] Doors;
    

    public override void Enter()
    {
        foreach (Door door in Doors)
        {
            door.OnUse += OnUse;
        }
        
        base.Enter();
    
        TodoListUI.Instance.SetText("Dirigez vous vers le batiment");
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