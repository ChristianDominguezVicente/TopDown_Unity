using System.Collections;
using UnityEngine;

public class ObjetoMovible : MonoBehaviour {
    public float velocidadMovimiento = 5f;
    private bool enMovimiento = false;

    public bool IntentarMover(Vector3 direccionEmpuje) {
        if (enMovimiento) return false;

        Vector3 nuevoDestino = transform.position + direccionEmpuje;

        if (!HayColisiones(nuevoDestino)) {
            StartCoroutine(Mover(nuevoDestino));
            return true;
        }

        return false;
    }

    private bool HayColisiones(Vector3 nuevoDestino) {
        return Physics2D.OverlapCircle(nuevoDestino, 0.2f) != null;
    }

    private IEnumerator Mover(Vector3 nuevoDestino) {
        enMovimiento = true;
        while (transform.position != nuevoDestino) {
            transform.position = Vector3.MoveTowards(transform.position, nuevoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
        enMovimiento = false;
    }
}
