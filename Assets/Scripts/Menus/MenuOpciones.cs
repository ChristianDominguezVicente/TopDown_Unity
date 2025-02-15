using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Toggle screen;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [SerializeField] private GameManagerSO gM;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject menuInicio;

    private Resolution[] resolutions; //Resoluciones disponibles por mi monitor.
    private List<string> resolutionOptions = new List<string>();

    private void Awake()
    {
        resolutions = Screen.resolutions; //Relleno el array con las resoluciones disponibles.

        foreach (var resolution in resolutions)
        {
            resolutionOptions.Add(resolution.width + "x" + resolution.height);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
            LoadMusic();
        else
            ChangeMusic();

        if (PlayerPrefs.HasKey("SFXVolume"))
            LoadSFX();
        else
            ChangeSFX();

        if (PlayerPrefs.HasKey("Screen"))
            LoadScreen();
        else
            Fullscreen();

        if (PlayerPrefs.HasKey("Quality"))
            LoadQuality();
        else
            ChangeQuality();

        if (PlayerPrefs.HasKey("Resolution"))
            LoadResolution();
        else
            ChangeResolution();
    }

    public void Fullscreen()
    {
        Screen.fullScreen = screen.isOn;
        if (screen.isOn == false)
            PlayerPrefs.SetInt("Screen", 1);
        else
            PlayerPrefs.SetInt("Screen", 0);
    }

    public void ChangeMusic()
    {
        float volumen = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volumen);
    }

    public void ChangeSFX()
    {
        float volumen = SFXSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volumen);
    }

    public void ChangeQuality()
    {
        int index = quality.value;
        PlayerPrefs.SetInt("Quality", index);
        QualitySettings.SetQualityLevel(index);
    }

    public void ChangeResolution()
    {
        int index = resolutionDropdown.value;
        Resolution chosenResolution = resolutions[index];
        PlayerPrefs.SetInt("Resolution", index);
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }

    private void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        ChangeMusic();
    }

    private void LoadSFX()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        ChangeSFX();
    }

    private void LoadScreen()
    {
        if (PlayerPrefs.GetInt("Screen") == 1)
            screen.isOn = false;
        else
            screen.isOn = true;
        Fullscreen();
    }

    private void LoadQuality()
    {
        quality.value = PlayerPrefs.GetInt("Quality");
        ChangeQuality();
    }

    private void LoadResolution()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        ChangeResolution();
    }
    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        gM.CambiarEstadoPlayer(InventoryUI.activeSelf);
        InventoryUI.SetActive(!InventoryUI.activeSelf);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Inicio");

        yield return new WaitUntil(() => operation.isDone);

        menuInicio.SetActive(true);
    }
}
