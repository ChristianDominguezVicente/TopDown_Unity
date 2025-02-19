using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    [SerializeField] float destroyTimer = 10f;
    [SerializeField] float controlDistance = 5f;
    [SerializeField] Transform player;
    private Coroutine timerCoroutine;

    void Start()
    {
        gameObject.transform.position = new Vector3(Mathf.Floor(gameObject.transform.position.x), Mathf.Floor(gameObject.transform.position.y), 0);
        gameObject.layer = 10;
        GetComponent<SpriteRenderer>().color = Color.white;
        timerCoroutine = StartCoroutine(DestroyTimer());
    }
    void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= controlDistance)
        {
            StopCoroutine(timerCoroutine);
            Debug.Log("Clicked on " + gameObject.name);
            gameObject.layer = 7;
            GetComponent<SpriteRenderer>().color = Color.red;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<MeleeAttack>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<MeleeAttack>().enabled = false;
            player.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }
}
