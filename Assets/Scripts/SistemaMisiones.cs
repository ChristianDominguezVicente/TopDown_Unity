using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;


public class SistemaMisiones : MonoBehaviour
{
    [SerializeField] private GameObject sistemaMisiones;
    [SerializeField] private TextMeshProUGUI[] nombreMisiones;
    [SerializeField] private Image[] fondoBarras;
    [SerializeField] private Image[] barraMisiones;
    [SerializeField] private TextMeshProUGUI[] nombreMisionesSecundarias;
    [SerializeField] private Image[] imagenMisionSecundaria;

    private int misionesDisponibles = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && "Inicio" != SceneManager.GetActiveScene().name)
        {
            sistemaMisiones.SetActive(!sistemaMisiones.activeSelf);
        }
    }

    public void NuevaMision(MisionSO datosMision)
    {
        Debug.Log("llegoAqui");
        //nombreMisiones[misionesDisponibles].gameObject.SetActive(true);
        //sistemaMisiones.GetComponent<TextMeshProUGUI>().SetActive(true);
        //nombreMisiones[misionesDisponibles].GetComponent<TextMeshProUGUI>().text = datosMision.nombre;
        nombreMisiones[misionesDisponibles].text = datosMision.nombre;
        //Debug.Log(nombreMisiones[misionesDisponibles].text);
        nombreMisiones[misionesDisponibles].gameObject.SetActive(true);
        fondoBarras[misionesDisponibles].gameObject.SetActive(true);
        datosMision.lugarEnCanvas = misionesDisponibles;
        //barraMisiones[misionesDisponibles].gameObject.SetActive(true);
        //barraMisiones[misionesDisponibles].
        misionesDisponibles++;
    }

    public void ActualizarMision(MisionSO datosMision, float avance)
    {
        Debug.Log("MellamamronActualizaR");
        barraMisiones[datosMision.lugarEnCanvas].fillAmount = avance / datosMision.numeroFases;
        barraMisiones[datosMision.lugarEnCanvas].gameObject.SetActive(true);

        if (avance == datosMision.numeroFases)
        {
            nombreMisionesSecundarias[datosMision.lugarEnCanvas].text = datosMision.secundaria;
            nombreMisionesSecundarias[datosMision.lugarEnCanvas].gameObject.SetActive(true);
            imagenMisionSecundaria[datosMision.lugarEnCanvas].sprite = datosMision.secundariaNoConseguida;
            imagenMisionSecundaria[datosMision.lugarEnCanvas].GetComponent<Image>().gameObject.SetActive(true);
        }
    }




}
