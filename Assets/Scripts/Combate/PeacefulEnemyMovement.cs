using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeacefulEnemyMovement : MovementBase
{
    [SerializeField] float wanderingSpeed = 5f;
    [SerializeField] float wanderingDelay = 0.5f;

    private Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Start()
    {
        StartCoroutine(RandomMovement());
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(wanderingDelay);

            if (!isMoving && !isStunned)
            {
                Vector2 randomDir = directions[Random.Range(0, directions.Length)];
                Vector3 targetPosition = transform.position + new Vector3(randomDir.x, randomDir.y, 0);

                if (Physics2D.OverlapCircle(targetPosition, 0.1f, obstacleLayer))
                    continue;

                yield return StartCoroutine(MoveToPosition(targetPosition));
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * wanderingSpeed;
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;
    }
}