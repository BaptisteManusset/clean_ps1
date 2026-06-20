public class GotoCarState : SimpleState
{
    public PlayerTriggerZone playerTriggerZone;

    public override void Enter()
    {
        base.Enter();
        TodoListUI.Instance.SetText("Retournez a votre voiture");

        playerTriggerZone.Entered += TriggerEntered;
    }

    private void TriggerEntered()
    {
        playerTriggerZone.Entered -= TriggerEntered;
        m_stateMachine.NextState();
    }
}