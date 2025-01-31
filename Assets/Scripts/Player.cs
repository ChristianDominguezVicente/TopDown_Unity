using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    private Vector3 ultimoInput;
    private Vector3 puntoInteraccion;
    private Collider2D colliderDelante; //Indica el collider que tenemos por delante.
    private Animator anim;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;

    private bool interactuando;

    public bool Interactuando { get => interactuando; set => interactuando = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = gM.NewPosition;
        anim.SetFloat("inputH", gM.NewOrientation.x);
        anim.SetFloat("inputV", gM.NewOrientation.y);
    }

    // Update is called once per frame
    void Update()
    {
        LecturaInputs();

        MovimientoYAnimaciones();
    }

    private void MovimientoYAnimaciones()
    {
        //Ejecuto el movimiento s�lo si estoy en una casilla y s�lo si hay input.
        if (!interactuando && !moviendo && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("andando", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);
            //Actualizo cual fue mi ultimo input, cual va a ser mi puntoDestino y cual es mi puntoInteraccion. 
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();

            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
        }
        else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("andando", false);
        }
    }

    private void LecturaInputs()
    {
        if (inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");
        }
        if (inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LanzarInteraccion();
        }
    }

    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            //Compruebo si lo que tengo delante es un iteractuable
            if (colliderDelante.TryGetComponent(out Interactuable interactuable))
            {
                //Si adem�s de ser interactuable es un item...
                if (interactuable.transform.TryGetComponent(out Item item))
                {
                    //Notifico a gM que tenemos un nuevo item.
                    gM.NewItem(item.ItemData);
                }
                interactuable.Interactuar();
            }
        }
    }

    IEnumerator Mover()
    {
        moviendo = true;
        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
        //Ante un nuevo destino, necesito refrescar de nuevo puntoInteraccion.
        puntoInteraccion = transform.position + ultimoInput;
        moviendo= false;
    }
    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
