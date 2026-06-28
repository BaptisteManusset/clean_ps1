public class ResetState : SimpleState
{
    public override void Enter()
    {
        base.Enter();
        
        BlackScreen.Instance.Fade();
        DayStatemachine fsm = (DayStatemachine)m_stateMachine;
        
        fsm.IncreaseDay();

        foreach (InteractorItem item in Library.GetAll())
        {
            item.ResetState();
        }
    }
}