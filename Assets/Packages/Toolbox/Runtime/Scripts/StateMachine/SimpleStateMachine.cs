using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateMachine : MonoBehaviour
{
    public bool IsPlaying => m_currentState != null;

    protected ISimpleState m_currentState;

    protected List<ISimpleState> m_states = new();
    public List<ISimpleState> States => m_states;

    public ISimpleState CurrentState => m_currentState;

    private bool m_isCompleted;
    public bool IsCompleted => m_isCompleted;

    private event Action<ISimpleState> m_onStateEntered;

    public event Action OnCompleted
    {
        add => m_onCompleted += value;
        remove => m_onCompleted -= value;
    }

    protected event Action m_onCompleted;

    private event System.Action<ISimpleState> m_onStateExited;

    public event System.Action<ISimpleState> OnStateExited
    {
        add => m_onStateExited += value;
        remove => m_onStateExited -= value;
    }
    
    private event System.Action<ISimpleState,ISimpleState> m_onStateChanged;
        public event System.Action<ISimpleState,ISimpleState> OnStateChanged
        {
            add => m_onStateChanged += value;
            remove => m_onStateChanged -= value;
        }

    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void OnDestroy()
    {
        Stop();
    }

    protected virtual void Setup()
    {
        if (m_states.Count == 0)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.TryGetComponent(out ISimpleState state)) continue;
                m_states.Add(state);
            }
        }

        foreach (ISimpleState state in m_states)
        {
            state.Setup(this);
        }
    }

    public virtual void Enter()
    {
        ChangeState(m_currentState);
    }

    public virtual void Exit()
    {
        if (m_currentState == null)
            return;

        m_isCompleted = true;
        m_onCompleted?.Invoke();
        Stop();
    }

    public virtual void Stop()
    {
        if (m_currentState == null)
            return;

        m_currentState.Exit();
        m_onStateExited?.Invoke(m_currentState);
        m_currentState = null;
    }

    public virtual void NextState()
    {
        if (m_isCompleted)
        {
            Debug.Log("{GetType().Name} already completed");
            return;
        }

        if (m_currentState != null && !m_currentState.NextStateRequested()) return;

        int index = m_states.IndexOf(m_currentState);
        if (index + 1 < m_states.Count)
        {
            ChangeState(m_states[index + 1]);
            return;
        }

        Exit();
    }

    public virtual void ChangeState(ISimpleState a_newState)
    {
        if (m_currentState != null)
        {
            m_currentState.Exit();
            m_onStateExited?.Invoke(m_currentState);
        }

        m_onStateChanged?.Invoke(m_currentState, a_newState);
        
        m_currentState = a_newState;
        m_currentState.Enter();
        m_onStateEntered?.Invoke(a_newState);
    }

    public void RequestReset()
    {
        if (m_currentState != null)
        {
            m_currentState.Exit();
            m_currentState = null;
        }

        m_isCompleted = false;
    }
}

public class SimpleStateMachine<TState> : SimpleStateMachine where TState : ISimpleState
{
    [SerializeField] protected TState m_defaultState;

    public void EnterDefault()
    {
        ChangeState(m_defaultState);
    }
}
