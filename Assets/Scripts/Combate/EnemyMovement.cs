using System.Collections;
using UnityEngine;

public class EnemyMovement : MovementBase
{
    [SerializeField] float wanderingSpeed = 2f;
    [SerializeField] float wanderingDelay = 1f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] float chaseSpeed = 5f;

    [SerializeField] CombateNivel combateNivel;
    [SerializeField] Transform player;
    [SerializeField] LayerMask visionObstacleLayer;

    private Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Start()
    {
        StartCoroutine(EnemyBehavior());
    }

    private IEnumerator EnemyBehavior()
    {
        while (true)
        {
            if (isStunned)
            {
                yield return null;
                continue;
            }

            if (PlayerInSight())
            {
                yield return StartCoroutine(ChasePlayer());
            }
            else
            {
                yield return new WaitForSeconds(wanderingDelay);

                Vector2 randomDir = directions[Random.Range(0, directions.Length)];
                Vector3 targetPosition = transform.position + new Vector3(randomDir.x, randomDir.y, 0);

                if (Physics2D.OverlapCircle(targetPosition, 0.1f, obstacleLayer))
                    continue;

                yield return StartCoroutine(MoveToPosition(targetPosition, wanderingSpeed));
            }
        }
    }

    private IEnumerator ChasePlayer()
    {
        while (PlayerInSight() && !isStunned)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Vector3 moveDirection;

            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                moveDirection = new Vector3(Mathf.Sign(directionToPlayer.x), 0, 0);
            else
                moveDirection = new Vector3(0, Mathf.Sign(directionToPlayer.y), 0);

            Vector3 targetPosition = transform.position + moveDirection;

            if (Physics2D.OverlapCircle(targetPosition, 0.1f, obstacleLayer))
            {
                yield return null;
                continue;
            }

            yield return StartCoroutine(MoveToPosition(targetPosition, chaseSpeed));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            if (isStunned)
                yield break;

            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }
        transform.position = targetPosition;
    }

    private bool PlayerInSight()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Debug.DrawRay(transform.position, direction * distanceToPlayer, Color.red, 0.1f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToPlayer, visionObstacleLayer);
            if (hit.collider != null)
            {
                Debug.Log("Raycast colision contra: " + hit.collider.name);
                return false;
            }
            return true;
        }
        return false;
    }

    public override void Die()
    {
        if(gameObject.TryGetComponent<EnemyMeleeAttack>(out EnemyMeleeAttack enemyAttack))
        {
            Destroy(enemyAttack);
        }
        if (gameObject.TryGetComponent<ControlEnemy>(out ControlEnemy enemyControl))
        {
            enemyControl.enabled = true;
        }
        combateNivel.EnemigoMatado();
        Destroy(this);
    }
}