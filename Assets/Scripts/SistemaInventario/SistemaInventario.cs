using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private ItemSlot[] slots;

    [SerializeField] private ItemSlot[] usableSlots;

    [SerializeField] private GameManagerSO gM;

    [SerializeField] private GameObject InventoryUI;

    [SerializeField] private GameObject panel;

    private List<ItemSO> myItems = new List<ItemSO>();

    private int itemsCollected = 0;

    private ItemInfo[] itemInfos;

    private static SistemaInventario instance;

    public ItemSlot[] Slots => slots;

    public ItemSlot[] GetSlots()
    {
        return slots;
    }

    public void ResetItemsCollected()
    {
        itemsCollected = 0;
    }

    public List<ItemSO> MyItems { get => myItems; set => myItems = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitSlots();
        }
        else
        {
            Destroy(gameObject);
        }  
    }

    private void InitSlots()
    {
        itemInfos = new ItemInfo[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            //Genero el array de las "infos" que vienen por defecto en los slots.
            itemInfos[i] = slots[i].GetComponentInChildren<ItemInfo>();
        }
    }

    private void OnEnable()
    {
        gM.OnNewItem += AddNewItem;
    }

    private void AddNewItem(ItemSO newItem)
    {
        int indexOfStackItem = myItems.FindIndex(x => x.itemName == newItem.itemName);

        if (indexOfStackItem != -1)
        {
            itemInfos[indexOfStackItem].UpdateStackItem();
        }
        else
        {
            myItems.Add(newItem);
            slots[itemsCollected].gameObject.SetActive(true);
            itemInfos[itemsCollected].FeedData(newItem);
            itemsCollected++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && "Inicio" != SceneManager.GetActiveScene().name && !panel.activeSelf)
        {
            gM.CambiarEstadoPlayer(InventoryUI.activeSelf);
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            usableSlots[0].GetComponentInChildren<ItemInfo>().UseItem();
        }
    }

    private void OnDisable()
    {
        gM.OnNewItem -= AddNewItem;
    }

    public void RestoreInventory(Dictionary<string, int> inventoryItems)
    {
        MyItems.Clear();
        itemsCollected = 0;

        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var entry in inventoryItems)
        {
            string itemName = entry.Key;
            int quantity = entry.Value;

            ItemSO item = ItemDatabase.GetItemByName(itemName);

            if (item != null)
            {
                if (!myItems.Exists(x => x.itemName == item.itemName))
                {
                    MyItems.Add(item);
                    slots[itemsCollected].gameObject.SetActive(true);
                    itemInfos[itemsCollected].FeedData(item);
                    itemInfos[itemsCollected].SetItemCount(quantity);
                    itemsCollected++;
                }
            }
        }
    }

    public ItemInfo GetItemInfo(ItemSO item)
    {
        int index = MyItems.IndexOf(item);
        if (index >= 0 && index < itemInfos.Length)
        {
            return itemInfos[index];
        }
        return null;
    }
}
