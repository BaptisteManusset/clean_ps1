public interface ISimpleState
{
    public void Setup(SimpleStateMachine a_stateMachine);
    public void Enter();

    public void Exit();

    /// <summary>
    /// called when a new state was requested
    /// </summary>
    /// <returns>return true if current state machine can go next state</returns>
    public bool NextStateRequested();
}