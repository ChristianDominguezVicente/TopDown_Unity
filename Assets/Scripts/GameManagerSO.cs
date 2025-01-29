using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO : ScriptableObject
{
    private Player player;
    private SistemaInventario inventario;

    public SistemaInventario Inventario { get => inventario; }

    private void OnEnable() //Llamadas por EVENTO.
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>();
        inventario = FindObjectOfType<SistemaInventario>();
    }

    public void CambiarEstadoPlayer(bool estado)
    {
        player.Interactuando = !estado;
    }
}
