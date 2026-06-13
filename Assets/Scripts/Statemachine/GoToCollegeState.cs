using System;
using UnityEngine;

public class GoToCollegeState : SimpleState
{
    [SerializeField]
    private Door[] Doors;

    private void Awake()
    {
        foreach (Door door in Doors)
        {
            door.OnUse += OnUse;
        }
    }

    public override void Enter()
    {
        base.Enter();
        TodoListUI.Instance.SetText("Dirigez vous vers le batiment");
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