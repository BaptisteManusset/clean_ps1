public class GotoIrrigatePlantState : SimpleState
{
    public string MissionText = "Arrosez toutes les plantes";


    public ItemData itemType;


    public override void Enter()
    {
        base.Enter();

        TodoListUI.Instance.SetText(MissionText);
        Inventory.Instance.OnChange += OnInventoryChange;
    }


    private void OnInventoryChange()
    {
        if (Inventory.Instance.GetCount(itemType) >= Library.GetCount(itemType))
        {
            m_stateMachine.NextState();
            Inventory.Instance.OnChange -= OnInventoryChange;
            return;
        }

        string text = $"Trash: {Inventory.Instance.GetCount(itemType)}/{Library.GetCount(itemType)}\n";

        TodoListUI.Instance.SetText(text);
    }
}