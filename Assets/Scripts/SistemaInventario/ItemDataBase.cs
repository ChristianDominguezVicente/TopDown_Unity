using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<string, ItemSO> itemDictionary;

    public static void Initialize()
    {
        if (itemDictionary == null)
        {
            itemDictionary = new Dictionary<string, ItemSO>();
            ItemSO[] items = Resources.LoadAll<ItemSO>("Items");

            foreach (var item in items)
            {
                itemDictionary[item.itemName] = item;
            }
        }
    }

    public static void ClearItems()
    {
        if (itemDictionary != null)
        {
            itemDictionary.Clear();
        }
    }

    public static ItemSO GetItemByName(string itemName)
    {
        if (itemDictionary == null)
        {
            Initialize();
        }

        return itemDictionary.TryGetValue(itemName, out ItemSO item) ? item : null;
    }
}