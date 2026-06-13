public class DayStatemachine : SimpleStateMachine
{
    public SimpleState DefaultState;
    private void OnEnable()
    {
        ChangeState(DefaultState);
    }
}