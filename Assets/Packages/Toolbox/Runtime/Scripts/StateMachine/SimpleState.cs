using System;
using UnityEngine;

public abstract class SimpleState : MonoBehaviour, ISimpleState
{
    public event Action Started
    {
        add => m_started += value;
        remove => m_started -= value;
    }

    private event Action m_started;

    public event Action Exited
    {
        add => m_exited += value;
        remove => m_exited -= value;
    }

    private event Action m_exited;

    public bool IsPlaying => gameObject.activeSelf;

    protected SimpleStateMachine m_stateMachine;

    public virtual void Setup(SimpleStateMachine a_stateMachine)
    {
        m_stateMachine = a_stateMachine;
        gameObject.SetActive(false);
    }

    public virtual void Enter()
    {
        gameObject.SetActive(true);
        m_started?.Invoke();
    }

    public virtual void Exit()
    {
        m_exited?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual bool NextStateRequested() => true;


}
