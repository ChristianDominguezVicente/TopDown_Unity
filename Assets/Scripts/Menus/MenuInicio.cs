using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicio : MonoBehaviour
{
    [SerializeField] private GameObject menuInicio;
    [SerializeField] private Button[] loadButtons;

    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/" + "save001.txt";
        CheckSaveFile();
    }

    private void CheckSaveFile()
    {
        if (File.Exists(savePath))
        {
            foreach (Button loadButton in loadButtons)
            {
                loadButton.interactable = true;
            }
        }
        else
        {
            foreach (Button loadButton in loadButtons)
            {
                loadButton.interactable = false;
            }
        }
    }

    public void NewGame()
    {
        ItemDatabase.ClearItems();

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            UpdateLoadButtons();
        }

        SceneManager.LoadScene("Pueblo");
        menuInicio.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateLoadButtons()
    {
        CheckSaveFile();
    }

}
