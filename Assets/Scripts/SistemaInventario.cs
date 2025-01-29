using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;
    
    private int itemsDisponibles = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            int indiceBoton = i;
            botones[i].onClick.AddListener(() => BotonClickado(indiceBoton));
        }
    }

    private void BotonClickado(int indiceBoton)
    {
        Debug.Log("Botón clickado " + indiceBoton);
    }
    public void NuevoItem(ItemSO datosItem)
    {
        //1. Activo un botón de mi inventario.
        botones[itemsDisponibles].gameObject.SetActive(true);
        //2. Alimento al botón con los datos alojados en el SO.
        botones[itemsDisponibles].GetComponent<Image>().sprite = datosItem.icono;
        itemsDisponibles++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            marcoInventario.SetActive(!marcoInventario.activeSelf);
        }
    }
}
