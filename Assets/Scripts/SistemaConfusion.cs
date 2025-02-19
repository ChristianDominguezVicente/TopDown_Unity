using System.Collections;
using UnityEngine;

public class SistemaConfusion : MonoBehaviour {
    [SerializeField] private float duracionConfusion = 4f;
    [SerializeField] private float multiplicadorVelocidadConfusion = 0.75f;
    private bool enConfusion = false;
    private Player player;

    private void Start() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C) && !enConfusion) {
            StartCoroutine(ActivarConfusion());
        }
    }

    private IEnumerator ActivarConfusion() {
        enConfusion = true;

        player.VelocidadMovimiento *= multiplicadorVelocidadConfusion;
        player.InvertirDirecciones(true);

        yield return new WaitForSeconds(duracionConfusion);

        player.VelocidadMovimiento /= multiplicadorVelocidadConfusion;
        player.InvertirDirecciones(false);

        enConfusion = false;
    }
}
