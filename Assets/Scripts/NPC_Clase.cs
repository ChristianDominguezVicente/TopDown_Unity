using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Clase : MonoBehaviour
{
    [SerializeField]
    private float velocidadMovimiento;

    [SerializeField]
    private float tiempoEntreEsperasMinimo;

    [SerializeField]
    private float tiempoEntreEsperasMaximo;

    [SerializeField]
    private float distanciaMaxima;

    [SerializeField]
    private float radioDeteccion;

    [SerializeField]
    private LayerMask queEsObstaculo;

    private Vector3 posicionObjetivo;
    private Vector3 posicionInicial;
    private Animator anim;

    private void Awake()
    {
        posicionInicial = transform.position;
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IrHaciaDestinoYEsperar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator IrHaciaDestinoYEsperar()
    {
        while(true) //Por siempre.
        {
            CalcularNuevoDestino();
            anim.SetBool("walking", true);
            
            while (transform.position != posicionObjetivo) //Va al ritmo de los frames pero corta bajo la condición establecida.
            {
                //Movimiento cinemático (no tiene físicas)
                transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime);
                yield return null;
            }
            anim.SetBool("walking", false);

            yield return new WaitForSeconds(Random.Range(tiempoEntreEsperasMinimo, tiempoEntreEsperasMaximo));
        }
    }

    private void CalcularNuevoDestino()
    {
        bool tileValido = false;
        int intentos = 0;

        while(!tileValido && intentos < 15)
        {
            int prob = Random.Range(0, 4);
            if(prob == 0)
            {
                posicionObjetivo = transform.position + Vector3.left;
                anim.SetFloat("h", -1);
                anim.SetFloat("v", 0);
            }
            else if(prob == 1)
            {
                posicionObjetivo = transform.position + Vector3.right;
                anim.SetFloat("h", 1);
                anim.SetFloat("v", 0);
            }
            else if (prob == 2)
            {
                posicionObjetivo = transform.position + Vector3.up;
                anim.SetFloat("v", 1);
                anim.SetFloat("h", 0);
            }
            else
            {
                posicionObjetivo = transform.position + Vector3.down;
                anim.SetFloat("v", -1);
                anim.SetFloat("h", 0);
            }

            tileValido = TileLibreYDentroDeDistancia();
            intentos++;
        }
    }

    private bool TileLibreYDentroDeDistancia()
    {
        if(Vector3.Distance(posicionInicial, posicionObjetivo) > distanciaMaxima)
        {
            return false;
        }
        else //El tile está dentro de la distancia máxima...
        {
            //Pero ahora voy a comprobar si está ocupado o no
            return !Physics2D.OverlapCircle(posicionObjetivo, radioDeteccion, queEsObstaculo);
        }
    }
}
