public class ResetState : SimpleState
{
    public override void Enter()
    {
        base.Enter();
        
        // BlackScreen.Instance.Fade();
        GameManager.Instance.globalStatemachine.IncreaseDay();

        foreach (InteractorItem item in Library.GetAll())
        {
            item.ResetState();
        }
        m_stateMachine.NextState();
    }
}