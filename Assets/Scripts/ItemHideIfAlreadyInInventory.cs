using System;
using UnityEngine;

public class ItemHideIfAlreadyInInventory : MonoBehaviour
{
    private InteractorItem m_item;

    private void Awake()
    {
        m_item = GetComponent<InteractorItem>();
    }

    private void Start()
    {
        Inventory.Instance.OnAddItem += OnChange;
        HideIfRequired(m_item.itemType.MaxCount, Inventory.Instance.GetCount(m_item.itemType));
    }

    private void OnEnable()
    {
        if (Inventory.Instance)
        {
            HideIfRequired(m_item.itemType.MaxCount, Inventory.Instance.GetCount(m_item.itemType));
        }
    }

    private void OnDestroy()
    {
        if (Inventory.Instance) Inventory.Instance.OnAddItem -= OnChange;
    }

    private void OnChange(Inventory.ItemAddedInfo a_info)
    {
        HideIfRequired(m_item.itemType.MaxCount, a_info.quantityInInventoryAfter);
    }

    private void HideIfRequired(int itemTypeMaxCount, int quantityInInventoryAfter)
    {
        if (itemTypeMaxCount <= quantityInInventoryAfter)
        {
            m_item.Hide();
        }
    }
}