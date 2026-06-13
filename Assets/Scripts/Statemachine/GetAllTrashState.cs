public class GetAllTrashState : SimpleState
{
    public ItemData itemType;

    public Conteneur[] Conteneurs;

    public override void Enter()
    {
        base.Enter();
        foreach (Conteneur conteneur in Conteneurs)
        {
            conteneur.OnUse += OnUse;
        }


        TodoListUI.Instance.SetText("Videz toutes les poubelles");
        Inventory.Instance.OnChange += SetText;
    }


    private void SetText()
    {
        TodoListUI.Instance.SetText($"Trash: {Inventory.Instance.GetCount(itemType)}/{Library.GetCount(itemType)}");
    }

    public override void Exit()
    {
        base.Exit();
        foreach (Conteneur conteneur in Conteneurs)
        {
            conteneur.OnUse -= OnUse;
        }

        Inventory.Instance.OnChange -= SetText;
    }

    private void OnUse()
    {
        int max = Library.GetCount(itemType);

        if (Inventory.Instance.GetCount(itemType) == max)
        {
            m_stateMachine.NextState();
        }
    }
}