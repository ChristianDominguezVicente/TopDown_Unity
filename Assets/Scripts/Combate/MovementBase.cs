using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class MovementBase : MonoBehaviour
{

    protected bool isStunned = false;
    protected bool isMoving = false;

    [SerializeField] Slider sliderVida;
    [SerializeField] int hp = 3;
    [SerializeField] protected LayerMask obstacleLayer;

    public virtual void ApplyKnockback(Vector2 knockbackDirection, float distance, float knockbackSpeed, float stunTime)
    {
        StartCoroutine(KnockbackRoutine(knockbackDirection.normalized, distance, knockbackSpeed, stunTime));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float distance, float knockbackSpeed, float stunTime)
    {
        isStunned = true;

        Vector3 currentPosition = transform.position;
        Vector3 validKnockbackPosition = currentPosition;
        bool foundValidTarget = false;

        int maxCells = Mathf.RoundToInt(distance);
        for (int d = maxCells; d >= 1; d--)
        {
            Vector3 posiblePosition = currentPosition + new Vector3(direction.x, direction.y, 0) * d;
            posiblePosition = new Vector3(Mathf.Round(posiblePosition.x), Mathf.Round(posiblePosition.y), currentPosition.z);

            if (!Physics2D.OverlapCircle(posiblePosition, 0.1f, obstacleLayer))
            {
                validKnockbackPosition = posiblePosition;
                foundValidTarget = true;
                break;
            }
        }

        if (!foundValidTarget)
        {
            validKnockbackPosition = currentPosition;
        }

        Vector3 startPosition = currentPosition;
        float elapsedTime = 0f;
        float duration = 1f / knockbackSpeed;

        isMoving = true;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, validKnockbackPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = validKnockbackPosition;

        yield return new WaitForSeconds(stunTime);

        isStunned = false;
        isMoving = false;
    }

    public virtual void GetDamaged(int damage)
    {
        hp -= damage;
        sliderVida.value = hp;
        if (hp <= 0)
        {
            sliderVida.gameObject.SetActive(false);
            Die();
        }
    }

    public virtual bool Curarse(int cura)
    {
        if(hp < sliderVida.maxValue)
        {
            hp += cura;
            sliderVida.value = hp;
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        Debug.Log("Muerto: " + gameObject.name);
        Destroy(gameObject);
    }
    public bool GetIsStunned()
    {
        return isStunned;
    }
}