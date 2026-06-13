public class GotoCarState : SimpleState
{
    
    public PlayerTriggerZone  playerTriggerZone;

    public override void Enter()
    {
        base.Enter();
        TodoListUI.Instance.SetText("Retournez a votre voiture");
    }
}