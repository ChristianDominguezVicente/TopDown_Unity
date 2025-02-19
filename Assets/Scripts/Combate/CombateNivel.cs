using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombateNivel : MonoBehaviour
{
    [SerializeField]
    private int enemigosObjetivo;
    private int enemigosMuertos = 0;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private string siguienteEscena;

    [SerializeField]
    private TextMeshProUGUI enemyProgressTxt;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyProgressTxt.text = enemigosMuertos + " / " + enemigosObjetivo;
    }

    public void EnemigoMatado()
    {
        enemigosMuertos++;
        if(enemigosMuertos >= enemigosObjetivo)
        {
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
            enemyProgressTxt.text = "COMPLETADO";
            return;
        }
        enemyProgressTxt.text = enemigosMuertos + " / " + enemigosObjetivo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //AVANZAR QUEST
            SceneManager.LoadSceneAsync(siguienteEscena);
        }
    }
}
