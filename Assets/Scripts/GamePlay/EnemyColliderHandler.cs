using UnityEngine;

public class EnemyColliderHandler : MonoBehaviour
{
    public CapsuleCollider2D normalCollider;
    public CapsuleCollider2D attackCollider;
    public void EnableAttackCollider()
    {
        normalCollider.enabled = false;
        attackCollider.enabled = true;
    }

    public void EnableNormalCollider()
    {
        normalCollider.enabled = true;
        attackCollider.enabled = false;
    }

}
