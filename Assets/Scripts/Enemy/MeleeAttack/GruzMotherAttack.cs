using UnityEngine;

public class GruzMotherAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockbackForce = new Vector2(5, 2);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable player = collision.GetComponent<Damageable>();
            if (player != null)
            {
                player.Hit(attackDamage, knockbackForce);
                Debug.Log("Player bị đánh trúng!");
            }
        }
    }
}
