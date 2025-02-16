using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class NPC : MonoBehaviour, Interactuable
{
    [Header("Dialogo")]
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField, TextArea(1, 5)] private string[] frasesSinAlcalde;
    [SerializeField, TextArea(1, 5)] private string[] frasesConAlcalde;
    [SerializeField, TextArea(1, 5)] private string[] frasesConMisionAsignada;
    [SerializeField, TextArea(1, 5)] private string[] frasesConMisionAcabada;

    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    private bool hablando = false;
    private int indiceActual = -1;

    [Header("Mision")]
    [SerializeField] private MisionSO misDatos;
    [SerializeField] private int clave;


    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float tiempoEntreEsperasMinimo;
    [SerializeField] private float tiempoEntreEsperasMaximo;
    [SerializeField] private float distanciaMaxima;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsObstaculo;
    [SerializeField] private LayerMask queEsPlayer;
    private Vector3 posicionObjetivo;
    private Vector3 posicionInical;


    private void Awake()
    {
        posicionInical = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.HablarAlcalde = false;
        for (int i = 0; i < 4; i++)
        {
            gameManager.EstadoMisiones[i] = -1;

        }
        for (int j = 0; j < 5; j++)
        {
            gameManager.MisionAcabada[j] = false;
            gameManager.MisionAsignada[j] = false;
        }
        StartCoroutine(IrHaciaDestinoYEsperar());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interactuar()
    {
        StopAllCoroutines();
        Debug.Log("Mi Posicion: " + transform.position);
        Reposicionar(transform.position);

        cuadroDialogo.SetActive(true);
        gameManager.CambiarEstadoPlayer(false);


        if (!hablando)
        {
            SiguienteFrase();
        }
        else
        {
            CompletarFrase();
        }

    }

    private void SiguienteFrase()
    {
        indiceActual++;

        if (!this.gameObject.CompareTag("Alcalde") && gameManager.MisionAcabada[clave] == true)
        {
            gameManager.MisionAsignada[clave] = false;
            if (indiceActual >= frasesConMisionAcabada.Length)
            {
                Debug.Log("Termine la misión: " + clave);
                TerminarDialogo();

            }
            else
            {
                StartCoroutine(EscribirFrase());
            }
        }
        else if (gameManager.MisionAsignada[clave])
        {
            if (indiceActual >= frasesConMisionAsignada.Length)
            {
                Debug.Log("Ya tengo la mision asignada: " + clave);
                TerminarDialogo();

            }
            else
            {
                StartCoroutine(EscribirFrase());
            }
        }
        else if (gameManager.HablarAlcalde)
        {
            if (indiceActual >= frasesConAlcalde.Length)
            {
                Debug.Log("Ya hablé con el alcalde y me asignan la mision: " + clave);
                TerminarDialogo();
                if (clave != 4)
                {
                    gameManager.Logros.NuevaMision(misDatos);
                    gameManager.MisionAsignada[clave] = true;
                    gameManager.MisionAsignada[4] = true;
                    for (int i = 0; i < 5; i++)
                    {
                        Debug.Log("Misison Asignada: " + i + " " + gameManager.MisionAsignada[i]);
                    }

                    gameManager.EstadoMisiones[clave] = 0;
                }
                //Debug.Log("Clave: " + gameManager.EstadoMisiones[clave]);
                //gameManager.HablarAlcalde = false;
            }
            else
            {
                StartCoroutine(EscribirFrase());
            }
        }
        else
        {
            if (indiceActual >= frasesSinAlcalde.Length)
            {
                Debug.Log("Todavía no he hablado con el alcalde");
                TerminarDialogo();

            }
            else
            {
                StartCoroutine(EscribirFrase());
            }
        }


    }

    private void TerminarDialogo()
    {
        //if (this.gameObject.CompareTag("Alcalde"))
        if (clave == 4)
        {
            Debug.Log("Estuve hablando con el alcalde");
            gameManager.HablarAlcalde = true;
        }
        hablando = false;
        textoDialogo.text = "";
        indiceActual = -1;
        cuadroDialogo.SetActive(false);
        StartCoroutine(IrHaciaDestinoYEsperar());
        gameManager.CambiarEstadoPlayer(true);
    }

    IEnumerator EscribirFrase()
    {
        hablando = true;
        textoDialogo.text = "";
        char[] caracteresFrase = null;
        //Debug.Log("YaHableConALcalde: " + gameManager.HablarAlcalde);
        if (gameManager.MisionAcabada[clave] == true && clave != 4)
        {
            caracteresFrase = frasesConMisionAcabada[indiceActual].ToCharArray();
        }
        else if (gameManager.MisionAsignada[clave])
        {
            caracteresFrase = frasesConMisionAsignada[indiceActual].ToCharArray();
        }
        else if (gameManager.HablarAlcalde)
        {
            caracteresFrase = frasesConAlcalde[indiceActual].ToCharArray();
        }

        else
        {
            caracteresFrase = frasesSinAlcalde[indiceActual].ToCharArray();
        }

        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
        hablando = false;

    }

    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = frasesSinAlcalde[indiceActual];
        hablando = false;

    }

    IEnumerator IrHaciaDestinoYEsperar()
    {
        posicionObjetivo = CalcularNuevoDestino();
        Debug.Log("Mi clave: " + clave);
        //while (true)
        while (!cuadroDialogo.activeSelf)
        {

            if (DestinoValido(posicionObjetivo))
            {
                while (transform.position != posicionObjetivo)
                {
                    transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(tiempoEntreEsperasMinimo, tiempoEntreEsperasMaximo));
                posicionObjetivo = CalcularNuevoDestino();
            }
            else
            {
                posicionObjetivo = CalcularNuevoDestino();
            }


        }
    }

    private Vector3 CalcularNuevoDestino()
    {

        int probabilidad = Random.Range(0, 4);


        if (probabilidad == 0)
        {
            posicionObjetivo = transform.position + Vector3.left;
        }
        else if (probabilidad == 1)
        {
            posicionObjetivo = transform.position + Vector3.right;
        }
        else if (probabilidad == 2)
        {
            posicionObjetivo = transform.position + Vector3.up;
        }
        else
        {
            posicionObjetivo = transform.position + Vector3.down;
        }
        return posicionObjetivo;
    }



    private bool TileDemasiadoLejos(Vector3 destino)
    {
        if (Vector3.Distance(posicionInical, destino) > distanciaMaxima)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TileConColision(Vector3 destino)
    {
        if (Physics2D.OverlapCircle(destino, radioDeteccion, queEsObstaculo))
        {
            return true;
        }
        else if (Physics2D.OverlapCircle(destino, radioDeteccion, queEsPlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DestinoValido(Vector3 destino)
    {
        if (TileDemasiadoLejos(destino) || TileConColision(destino))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Reposicionar(Vector3 posicion)
    {
        float nuevaPosX = 0.5f;
        float nuevaPosY = 0.5f;
        Vector3 tuSitio;
        if (posicion.y >= posicion.x - gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y &&
            posicion.y >= -1 * posicion.x + gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y)
        {
            nuevaPosX = gameManager.MyPlayer.transform.position.x;
            nuevaPosY = gameManager.MyPlayer.transform.position.y + 1;
        }
        else if (posicion.y <= posicion.x - gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y &&
            posicion.y <= -1 * posicion.x + gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y)
        {
            nuevaPosX = gameManager.MyPlayer.transform.position.x;
            nuevaPosY = gameManager.MyPlayer.transform.position.y + -1;
        }
        else if (posicion.y < posicion.x - gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y &&
            posicion.y > -1 * posicion.x + gameManager.MyPlayer.transform.position.x + gameManager.MyPlayer.transform.position.y)
        {
            nuevaPosX = gameManager.MyPlayer.transform.position.x + 1;
            nuevaPosY = gameManager.MyPlayer.transform.position.y;
        }
        else
        {
            nuevaPosX = gameManager.MyPlayer.transform.position.x - 1;
            nuevaPosY = gameManager.MyPlayer.transform.position.y;
        }

        //if (posicion.x - gameManager.MyPlayer.transform.position.x == 1 || posicion.x - gameManager.MyPlayer.transform.position.x == -1)
        //{
        //    nuevaPosY = gameManager.MyPlayer.transform.position.y;
        //    nuevaPosX = transform.position.x;
        //}

        //if (posicion.y - gameManager.MyPlayer.transform.position.y == 1 || posicion.y - gameManager.MyPlayer.transform.position.y == -1)
        //{
        //    nuevaPosX = gameManager.MyPlayer.transform.position.x;
        //    nuevaPosY = transform.position.y;
        //}

        Debug.Log("Nueva posicion X: " + nuevaPosX);
        Debug.Log("Nueva posicion Y: " + nuevaPosY);
        tuSitio = new Vector3(nuevaPosX, nuevaPosY, 0);
        StartCoroutine(Reposicionando(tuSitio));
        //transform.Translate(new Vector3(nuevaPosX, nuevaPosY, 0);velocidadMovimiento*time.deltaT)


    }
    IEnumerator Reposicionando(Vector3 destino)
    {
        while (transform.position != destino)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);
        }
        yield return null;
    }
}

