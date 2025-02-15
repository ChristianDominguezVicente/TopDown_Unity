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
        if(myItems.Contains(newItem))
        {
            //Actualizarlo
            int indexOfStackItem = myItems.FindIndex(x => x.Equals(newItem));
            itemInfos[indexOfStackItem].UpdateStackItem();
        }
        else
        {
            myItems.Add(newItem); //Añado nueva información a la lista.
            slots[itemsCollected].gameObject.SetActive(true); //Enciendo el slot correspondiente

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
}
