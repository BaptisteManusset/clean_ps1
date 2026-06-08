using UnityEngine;

public class Conteneur : MonoBehaviour, IUsable
{
    public ItemData ItemData;

    public void Use()
    {
        TodoListManager.Instance.Clear(ItemData);

    }

    public void ExitUse()
    {
    }
}