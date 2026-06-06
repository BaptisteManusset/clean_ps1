using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();


    public void AddItem(ItemData item, int a_count)
    {
        if (!items.TryAdd(item, a_count))
        {
            items[item] += a_count;
        }
    }

    private void OnGUI()
    {
        string list = "";

        foreach (KeyValuePair<ItemData, int> keyValuePair in items)
        {
            list += $" {keyValuePair.Key.name} {keyValuePair.Value}\n";
        }

        GUI.Label(new Rect(10, 10, 100, 300),list);
    }

    public bool Contains(ItemData a_key)
    {
        return items.ContainsKey(a_key) && items[a_key] > 0;
    }

    public void Decrease(ItemData a_key, int a_amount = 1)
    {
        items[a_key] -= a_amount;
        items[a_key] = Mathf.Max(items[a_key], 0);   
    }
}