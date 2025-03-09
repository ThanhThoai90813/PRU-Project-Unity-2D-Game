using UnityEngine;

public class Attack : MonoBehaviour
{
    public int baseAttackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            Animator enemyAnimator = collision.GetComponent<Animator>();
            if (enemyAnimator != null && enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("block"))
            {
                Debug.Log(collision.name + " blocked the attack!");
                return;
            }

            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2 (-knockback.x, knockback.y);

            //hitTrigger the target
            bool gotHit = damageable.Hit(baseAttackDamage,knockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + baseAttackDamage);
            }

        }
    }
}
