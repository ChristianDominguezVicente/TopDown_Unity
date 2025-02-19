using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] AttackHitbox attackPrefab;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float attackDuration = 0.3f;
    [SerializeField] float attackCooldown = 0.5f;

    private PlayerMovement movementScript;
    private bool isAttacking = false;
    private bool isOnCooldown = false;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !isOnCooldown && !movementScript.GetIsStunned())
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        movementScript.enabled = false;

        Vector2 attackDirection = movementScript.GetLastMoveDirection();
        Vector3 attackPosition = transform.position + new Vector3(attackDirection.x, attackDirection.y, 0);
        Quaternion attackRotation = Quaternion.identity;
        if(attackDirection.y != 0)
        {
            attackRotation = Quaternion.Euler(0, 0, 90);
        }

        AttackHitbox attackInstance = Instantiate(attackPrefab, attackPosition, attackRotation);
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
}