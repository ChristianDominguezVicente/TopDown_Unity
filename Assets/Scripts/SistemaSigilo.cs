using System.Collections;
using UnityEngine;

public class SistemaSigilo : MonoBehaviour {
    [SerializeField] private float duracionSigilo;
    [SerializeField] private float cooldownSigilo;
    [SerializeField] private float transparencia;
    [SerializeField] private float multiplicadorVelocidadSigilo = 1.5f;

    private SpriteRenderer spriteRenderer;
    private bool enSigilo = false;
    private bool enCooldown = false;
    private Coroutine sigiloCoroutine;
    private Player player;

    public bool EnSigilo { get => enSigilo; }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (enSigilo) {
                DesactivarSigilo();
            }
            else if (!enCooldown) {
                sigiloCoroutine = StartCoroutine(ActivarSigilo());
            }
        }
    }

    private IEnumerator ActivarSigilo() {
        enSigilo = true;
        enCooldown = true;
        Color colorOriginal = spriteRenderer.color;
        spriteRenderer.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, transparencia);

        if (player != null) {
            player.VelocidadMovimiento *= multiplicadorVelocidadSigilo;
        }

        yield return new WaitForSeconds(duracionSigilo);

        if (enSigilo) {
            DesactivarSigilo();
        }
    }

    private void DesactivarSigilo() {
        if (sigiloCoroutine != null) {
            StopCoroutine(sigiloCoroutine);
        }
        enSigilo = false;
        Color colorOriginal = spriteRenderer.color;
        spriteRenderer.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 1f);

        if (player != null) {
            player.VelocidadMovimiento /= multiplicadorVelocidadSigilo;
        }

        StartCoroutine(CooldownSigilo());
    }

    private IEnumerator CooldownSigilo() {
        yield return new WaitForSeconds(cooldownSigilo);
        enCooldown = false;
    }
}
