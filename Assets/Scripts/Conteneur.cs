using JSAM;
using UnityEngine;

public class Conteneur : MonoBehaviour, IUsable
{
    public ItemData ItemData;
    public SoundFileObject lockSound;

    public void Use()
    {
        TodoListManager.Instance.Clear(ItemData);
        lockSound?.Play(transform.position);
    }

    public void ExitUse()
    {
    }
}