using UnityEngine;
using System.Collections;

public class SpikeDamage : MonoBehaviour
{
    public int damageAmount = 5; // Sát thương gây ra mỗi lần chạm
    public float damageInterval = 3f; // Khoảng thời gian giữa mỗi lần gây sát thương

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable player = collision.GetComponent<Damageable>();
            if (player != null)
            {
                StartCoroutine(ApplyDamageOverTime(player));
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(Damageable player)
    {
        while (player != null && player.IsAlive)
        {
            player.Hit(damageAmount, Vector2.zero); // Gây sát thương
            yield return new WaitForSeconds(damageInterval); // Đợi trước khi gây sát thương tiếp theo
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines(); // Ngừng gây sát thương khi Player rời khỏi Spike
        }
    }
}
