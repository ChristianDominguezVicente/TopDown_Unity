using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision : MonoBehaviour, Interactuable
{
    //[SerializeField] private int tamanhoMision;
    //[SerializeField] private int avanceMision;
    [SerializeField] private int numeroMision;
    //[SerializeField] private GameObject sistemaMisiones;
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private MisionSO misDatos;
    void Interactuable.Interactuar()
    {
        if (gameManager.EstadoMisiones[numeroMision] < misDatos.numeroFases)
        {
            gameManager.EstadoMisiones[numeroMision]++;
            gameManager.Logros.ActualizarMision(misDatos, gameManager.EstadoMisiones[numeroMision]);
        }
        if (misDatos.numeroFases == gameManager.EstadoMisiones[numeroMision])
        {
            gameManager.MisionAcabada[numeroMision] = true;
            Debug.Log("Acabaste la misión número: " + numeroMision);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
