using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] private ItemSO misDatos;
    [SerializeField] private GameManagerSO gameManager;
    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        Destroy(this.gameObject);   
    }
}
