using UnityEngine;

public class Item : MonoBehaviour, IUsable
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

public interface IUsable
{
    public void Use();
    public void ExitUse();
}