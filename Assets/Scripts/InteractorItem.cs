using System;
using JSAM;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public class InteractorItem : MonoBehaviour, IUsable
{
    public int count = 1;
    private int defaultCount = -1;
    public ItemData itemType;

    public GameObject defaultVisual;
    public GameObject usedVisual;

    public SoundFileObject useSound;

    public SpriteRenderer SpriteRenderer;
    public event Action Used;

    private void Awake()
    {
        defaultCount = count;
        ResetState();
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


    public void ResetState()
    {
        defaultVisual.SetActive(true);
        usedVisual.SetActive(false);
        count = defaultCount;
    }

    private void OnValidate()
    {
        if (itemType == null || itemType.image == null || SpriteRenderer == null) return;

        Undo.RecordObject(this, "Update key visual");
        SpriteRenderer.sprite = itemType.image;
    }

    [EditorButton]
    private void RenameAsset()
    {
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), itemType.name);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!enabled) return;
        Handles.Label(transform.position, itemType.name);
    }
#endif
}

public interface IUsable
{
    public void Use();
    public void ExitUse();
}