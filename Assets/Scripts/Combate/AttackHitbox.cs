using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] int attackDamage = 1;
    [SerializeField] bool doesKnockback = true;
    [SerializeField] float attackKnockback = 1.0f;
    [SerializeField] float knockBackSpeed = 5.0f;
    [SerializeField] float stunTime = 1.0f;
    [SerializeField] LayerMask attackLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & attackLayer) != 0)
        {
            if (collision.TryGetComponent<MovementBase>(out MovementBase hitTarget))
            {
                hitTarget.GetDamaged(attackDamage);

                if(doesKnockback)
                {
                    Vector2 hitDirection = (hitTarget.gameObject.transform.position - transform.position).normalized;
                    hitTarget.ApplyKnockback(hitDirection, attackKnockback, knockBackSpeed, stunTime);
                }
            }
            Debug.Log(gameObject.name + " golpea a: " +  collision.gameObject.name);
        }
    }

    public void SetAttackLayer(LayerMask attackLayer)
    {
        this.attackLayer = attackLayer;
    }
}