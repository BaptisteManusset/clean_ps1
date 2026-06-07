using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int count = 1;

    public void Use()
    {
        Inventory.Instance.AddItem(data,count);
        gameObject.SetActive(false);
    }

    public void ExitUse()
    {
    }
}