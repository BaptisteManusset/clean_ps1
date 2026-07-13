
public class DayStateMachine : SimpleStateMachine, SubStateMachine
{
    // [SerializeField] private SimpleState defaultState;
    
    public void StartDefaultState()
    {
        NextState();
    }
}