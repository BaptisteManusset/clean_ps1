using System;
using JSAM;
using UnityEngine;

[SelectionBase]
public class InteractorItem : MonoBehaviour, IUsable
{
    public int count = 1;
    public ItemData itemType;

    public GameObject defaultVisual;
    public GameObject usedVisual;

    public SoundFileObject useSound;

    public Zone current;


    public event Action Used;

    private void Awake()
    {
        ResetState();
        current = GetComponentInParent<Zone>();
        Library.Add(this);
    }

    public void Use()
    {
        if (count == 0) return;
        Inventory.Instance.AddItem(itemType, count);
        count = 0;

        defaultVisual.SetActive(false);
        usedVisual.SetActive(true);

        Used?.Invoke();
        useSound?.Play();
    }

    public void ExitUse()
    {
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = count != 0 ? Color.red : Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    public void ResetState()
    {
        defaultVisual.SetActive(true);
        usedVisual.SetActive(false);
        count = 1;
    }
}

public interface IUsable
{
    public void Use();
    public void ExitUse();
}