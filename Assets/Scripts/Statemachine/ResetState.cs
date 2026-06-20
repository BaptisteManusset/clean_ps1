public class ResetState : SimpleState
{
    public override void Enter()
    {
        base.Enter();
        foreach (InteractorItem item in Library.GetAll())
        {
            item.ResetState();    
        }
    }
}