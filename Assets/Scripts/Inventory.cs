using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SceneSingleton<Inventory>
{
    Dictionary<ItemData, int> items = new();

    public event Action<int> OnAddItem;
    public event Action OnClearItem;
    public event Action OnChange;


    public void AddItem(ItemData item, int count = 1)
    {
        if (!items.TryAdd(item, count))
        {
            items[item] += count;
        }

        OnAddItem?.Invoke(items[item]);
        OnChange?.Invoke();
    }

    private void OnGUI()
    {
        string list = "";

        foreach (KeyValuePair<ItemData, int> keyValuePair in items)
        {
            list += $" {keyValuePair.Key.name} {keyValuePair.Value}\n";
        }

        GUI.Label(new Rect(10, 10, 100, 300), list);
    }

    public bool Contains(ItemData a_key)
    {
        return items.ContainsKey(a_key) && items[a_key] > 0;
    }

    public int GetCount(ItemData a_key)
    {
        return items.GetValueOrDefault(a_key, 0);
    }

    public void Decrease(ItemData a_key, int a_amount = 1)
    {
        items[a_key] -= a_amount;
        items[a_key] = Mathf.Max(items[a_key], 0);
    }

    public void ClearItem(ItemData itemData)
    {
        items[itemData] = 0;
        OnClearItem?.Invoke();
        OnChange?.Invoke();
    }
}