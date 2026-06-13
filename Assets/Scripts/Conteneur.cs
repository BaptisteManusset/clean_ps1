using System;
using JSAM;
using UnityEngine;

public class Conteneur : MonoBehaviour, IUsable
{
    public ItemData ItemData;
    public SoundFileObject lockSound;

    public event Action OnUse;

    public void Use()
    {
        if (!Inventory.Instance.Contains(ItemData)) return;
        lockSound?.Play(transform.position);
        OnUse?.Invoke();
        // Inventory.Instance.ClearItem(ItemData);
    }

    public void ExitUse()
    {
    }
}