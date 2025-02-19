using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] AttackHitbox attackPrefab;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float attackDuration = 0.3f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] Transform player;

    private MovementBase movementScript;
    private AttackHitbox attackInstance;

    private bool isAttacking = false;
    private bool isOnCooldown = false;

    void Start()
    {
        movementScript = GetComponent<MovementBase>();
    }

    void Update()
    {
        if (player != null && !isAttacking && !isOnCooldown && !movementScript.GetIsStunned())
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        movementScript.enabled = false;

        Vector2 attackDirection = (player.position - transform.position).normalized;

        Vector3 attackPosition = transform.position + new Vector3(attackDirection.x, attackDirection.y, 0);
        Quaternion attackRotation = Quaternion.identity;
        if (attackDirection.y != 0)
        {
            attackRotation = Quaternion.Euler(0, 0, 90);
        }

        attackInstance = Instantiate(attackPrefab, attackPosition, attackRotation);
        attackInstance.SetAttackLayer(attackLayer);
        attackInstance.gameObject.transform.parent = transform;

        yield return new WaitForSeconds(attackDuration);

        Destroy(attackInstance.gameObject);
        isAttacking = false;
        movementScript.enabled = true;

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    private void OnDestroy()
    {
        if(attackInstance != null)
        {
            Destroy(attackInstance.gameObject);
        }
    }
}