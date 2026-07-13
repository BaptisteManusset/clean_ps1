public class SubStateMachineState : SimpleState
{
    public DayStateMachine SubStateMachine;

    private void Awake()
    {
        SubStateMachine = GetComponent<DayStateMachine>();
    }

    public override void Enter()
    {
        base.Enter();
        SubStateMachine.StartDefaultState();
        SubStateMachine.OnCompleted += OnCompleted;
    }

    private void OnCompleted()
    {
        SubStateMachine.OnCompleted -= OnCompleted;
        m_stateMachine.NextState();
    }


    public override void Exit()
    {
        base.Exit();
    }
}