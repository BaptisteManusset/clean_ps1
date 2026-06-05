using System.Collections.Generic;
using UnityCommunity.UnitySingleton;

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
}