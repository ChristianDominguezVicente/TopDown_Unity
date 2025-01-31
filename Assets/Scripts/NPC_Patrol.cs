using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
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
            anim.SetBool("andando", true);
            
            while (transform.position != posicionObjetivo) //Va al ritmo de los frames pero corta bajo la condición establecida.
            {
                //Movimiento cinemático (no tiene físicas)
                transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime);
                yield return null;
            }
            anim.SetBool("andando", false);

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
                anim.SetFloat("inputH", -1);
                anim.SetFloat("inputV", 0);
            }
            else if(prob == 1)
            {
                posicionObjetivo = transform.position + Vector3.right;
                anim.SetFloat("inputH", 1);
                anim.SetFloat("inputV", 0);
            }
            else if (prob == 2)
            {
                posicionObjetivo = transform.position + Vector3.up;
                anim.SetFloat("inputV", 1);
                anim.SetFloat("inputH", 0);
            }
            else
            {
                posicionObjetivo = transform.position + Vector3.down;
                anim.SetFloat("inputV", -1);
                anim.SetFloat("inputH", 0);
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
