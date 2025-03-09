using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    [Header("Bullet Settings")]
    public float force = 5f;
    private float timer;
    public int damage = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2 (direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Damageable dmg = other.gameObject.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.Hit(damage, Vector2.zero);
            }
            Destroy(gameObject);
        }

    }

}
