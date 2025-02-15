using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    [SerializeField] private GameObject menuInicio;

    public void NewGame()
    {
        SceneManager.LoadScene("Pueblo");
        menuInicio.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
