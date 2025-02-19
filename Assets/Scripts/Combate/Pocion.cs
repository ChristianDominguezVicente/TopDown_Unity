using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : MonoBehaviour
{
    [SerializeField]
    private int curacion = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemigo"))
        {
            if (collision.GetComponent<PlayerMovement>().Curarse(curacion))
            {
                Destroy(gameObject);
            }
        }
    }
}
