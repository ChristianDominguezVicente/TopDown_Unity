using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO : ScriptableObject
{
    public event Action<ItemSO> OnNewItem;

    private Player myPlayer;
    private SistemaInventario inventario;
    private SistemaMisiones logros;
    private bool hablarAlcalde;
    private bool[] misionAsignada = new bool[5];
    private float[] estadoMisiones = new float[4];
    private bool[] misionAcabada = new bool[5];


    [NonSerialized] private Dictionary<int, bool> items = new Dictionary<int, bool>();

    //Permitir que este dato Unity lo resetee entre partidas (al igual que los scripts Monobehaviour)
    [NonSerialized] private Vector3 newPosition = new Vector3(-6.5f, 2.5f, 0);
    [NonSerialized] private Vector2 newOrientation = new Vector2(0, -1);

    [NonSerialized] private int collectedCoins;

    private Player currentPlayer;

    public SistemaInventario Inventario { get => inventario; }

    public Player MyPlayer { get => myPlayer; set => myPlayer = value; }
    public SistemaMisiones Logros { get => logros; set => logros = value; }

    public bool HablarAlcalde { get => hablarAlcalde; set => hablarAlcalde = value; }
    public float[] EstadoMisiones { get => estadoMisiones; set => estadoMisiones = value; }
    public bool[] MisionAcabada { get => misionAcabada; set => misionAcabada = value; }
    public bool[] MisionAsignada { get => misionAsignada; set => misionAsignada = value; }
    public Vector3 NewPosition { get => newPosition; }
    public Vector2 NewOrientation { get => newOrientation; }
    public int CollectedCoins { get => collectedCoins; set => collectedCoins = value; }
    public Dictionary<int, bool> Items { get => items; set => items = value; }

    public Player CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

    private void OnEnable() //Llamadas por EVENTO.
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        myPlayer = FindObjectOfType<Player>();
        inventario = FindObjectOfType<SistemaInventario>();
    }
    public void CambiarEstadoPlayer(bool estado)
    {
        myPlayer.Interactuando = !estado;
    }
    public void LoadNewScene(Vector3 newPosition, Vector2 newOrientation, int newSceneIndex)
    {
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);
    }

    public void NewItem(ItemSO itemData)
    {
        OnNewItem?.Invoke(itemData);
    }
}