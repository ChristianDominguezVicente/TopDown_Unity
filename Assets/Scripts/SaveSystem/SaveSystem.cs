using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private GameManagerSO gM;

    [SerializeField] private TMP_Text textoMonedas;

    [SerializeField] private GameObject menuInicio;

    private MenuInicio menuInicioScript;

    private void Start()
    {
        menuInicioScript = FindObjectOfType<MenuInicio>();
    }

    public void OnSaveButtonClicked()
    {
        Save();
    }

    public void OnLoadButtonClicked()
    {
        StartCoroutine(LoadSceneAndData());
    }

    private IEnumerator LoadSceneAndData()
    {
        PersistentData loadedData = Load();
        gM.Items = loadedData.Items; //Sirve para que los objetos ya recogidos no vuelvan a spawnear ENTRE SESIONES DE JUEGO.

        AsyncOperation operation = SceneManager.LoadSceneAsync(loadedData.CurrentSceneIndex);

        yield return new WaitUntil(() => operation.isDone);

        textoMonedas.text = "Coins: " + loadedData.Monedas;
        gM.CurrentPlayer.transform.position = new Vector3(loadedData.UltimaPosicionPlayerX, loadedData.UltimaPosicionPlayerY);
        gM.CurrentPlayer.Anim.SetFloat("inputH", loadedData.XOrientation);
        gM.CurrentPlayer.Anim.SetFloat("inputV", loadedData.YOrientation);

        if ("Inicio" != SceneManager.GetActiveScene().name)
        {
            menuInicio.SetActive(false);
        }

        ItemDatabase.Initialize();
        SistemaInventario inventario = FindObjectOfType<SistemaInventario>();
        inventario.RestoreInventory(loadedData.InventoryItems);
    }


    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "save001.txt";

        FileStream stream = new FileStream(path, FileMode.Create);

        PersistentData dataToSave = new PersistentData(gM);

        //GUARDADO.
        formatter.Serialize(stream, dataToSave);

        stream.Close();

        if (menuInicioScript != null)
        {
            menuInicioScript.UpdateLoadButtons();
        }
    }
    public PersistentData Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "save001.txt";

        FileStream stream = new FileStream(path, FileMode.Open);

        //CARGADO.
        PersistentData loadedData = formatter.Deserialize(stream) as PersistentData;

        stream.Close();

        panel.SetActive(false);

        return loadedData;
    }
}