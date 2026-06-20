using System.Collections;
using System.Collections.Generic;

public static class Library
{
    private static Dictionary<ItemData, List<InteractorItem>> m_datas = new();
    private static List<InteractorItem> m_all = new();


    public static void Add(InteractorItem itemData)
    {
        if (!m_datas.ContainsKey(itemData.itemType))
        {
            m_datas.Add(itemData.itemType, new List<InteractorItem>());
        }

        m_datas[itemData.itemType].Add(itemData);

        m_all.Add(itemData);
    }

    public static int GetCount(ItemData itemData)
    {
        return m_datas.TryGetValue(itemData, out List<InteractorItem> data) ? data.Count : 0;
    }

    public static List<InteractorItem> GetAll()
    {
        return m_all;
    }
}