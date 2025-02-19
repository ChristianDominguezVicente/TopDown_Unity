using System.Collections;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MovementBase
{
    [SerializeField] float moveSpeed = 5f;

    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.right;

    void Update()
    {
        if (!isMoving && !isStunned)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Solo movimiento en una direccion
            if (input.x != 0)
                input.y = 0;

            if (input != Vector2.zero)
            {
                lastMoveDirection = input;
                Vector3 movePosition = transform.position + new Vector3(input.x, input.y, 0);
                StartCoroutine(Move(movePosition));
            }
        }
    }

    private IEnumerator Move(Vector3 movePosition)
    {
        isMoving = true;

        if (Physics2D.OverlapCircle(movePosition, 0.1f, obstacleLayer))
        {
            isMoving = false;
            yield break;
        }

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, movePosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        transform.position = movePosition;
        isMoving = false;
    }

    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}   