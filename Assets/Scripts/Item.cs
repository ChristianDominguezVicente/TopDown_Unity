using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] protected GameManagerSO gameManager;
    [SerializeField] private int uniqueId;

    public ItemSO ItemData { get => itemData; }

    public virtual void Interactuar()
    {
        gameManager.Items[uniqueId] = false; //No vamos a volver a respawnear.
        Destroy(gameObject);   
    }
    protected void Start()
    {
        //Busco en el listado de items de gameManager si ya tiene contenido mi ID
        if (gameManager.Items.ContainsKey(uniqueId))
        {
            if (gameManager.Items[uniqueId] == false)
            {
                Destroy(gameObject);
            }
        }
        else //y Si no estoy, significa que es la primera vez que "nazco".
        {
            gameManager.Items.Add(uniqueId, true);
        }
    }
}
