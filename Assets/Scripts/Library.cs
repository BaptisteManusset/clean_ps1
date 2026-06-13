using System.Collections.Generic;
using UnityEngine;

public static class Library
{
    private static Dictionary<ItemData, int> Dictionary = new();


    public static void Add(ItemData itemData)
    {

        if (!Dictionary.TryAdd(itemData, 1))
        {
            Dictionary[itemData] += 1;
        }
    }

    public static int GetCount(ItemData itemData)
    {
        return Dictionary.GetValueOrDefault(itemData, 0);
    }
}