using UnityEngine;

public class PhysicalItem : MonoBehaviour, IUsable
{
    public int count = 1;
    public ItemData itemType;


    public void Use()
    {
        if (count == 0) return;
        Inventory.Instance.AddItem(itemType, count);
        count = 0;
    }

    public void ExitUse()
    {
    }
}