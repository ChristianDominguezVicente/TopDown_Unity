using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PersistentData
{
    private int monedas;
    private float ultimaPosicionPlayerX, ultimaPosicionPlayerY;
    private Dictionary<int, bool> items = new Dictionary<int, bool>();
    private int currentSceneIndex;
    private float xOrientation, yOrientation;

    private Dictionary<string, int> inventoryItems = new Dictionary<string, int>();

    public PersistentData(GameManagerSO gM)
    {
        monedas = gM.CollectedCoins;
        ultimaPosicionPlayerX = gM.CurrentPlayer.transform.position.x;
        ultimaPosicionPlayerY = gM.CurrentPlayer.transform.position.y;
        items = gM.Items;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        xOrientation = gM.CurrentPlayer.Anim.GetFloat("inputH");
        yOrientation = gM.CurrentPlayer.Anim.GetFloat("inputV");

        inventoryItems = new Dictionary<string, int>();

        foreach (var item in gM.Inventario.MyItems)
        {
            int index = gM.Inventario.MyItems.IndexOf(item);
            if (index >= 0 && index < gM.Inventario.Slots.Length)
            {
                ItemInfo itemInfo = gM.Inventario.Slots[index].GetComponentInChildren<ItemInfo>();
                if (itemInfo != null)
                {
                    if (inventoryItems.ContainsKey(item.itemName))
                    {
                        inventoryItems[item.itemName] += itemInfo.ItemCount;
                    }
                    else
                    {
                        inventoryItems[item.itemName] = itemInfo.ItemCount;
                    }
                }
            }
        }
    }

    public int Monedas { get => monedas; }
    public float UltimaPosicionPlayerX { get => ultimaPosicionPlayerX; }
    public float UltimaPosicionPlayerY { get => ultimaPosicionPlayerY; }
    public Dictionary<int, bool> Items { get => items; }
    public int CurrentSceneIndex { get => currentSceneIndex; }

    public float XOrientation { get => xOrientation; }
    public float YOrientation { get => yOrientation; }
    public Dictionary<string, int> InventoryItems { get => inventoryItems; }
}
