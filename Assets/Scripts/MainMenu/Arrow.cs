using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 10;
    private int direction = 1;
    public Vector2 knockback = Vector2.zero;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Hit(damage, knockback);
            }
            Destroy(gameObject); 
        }
    }

    public void SetDirection(int dir)
    {
        direction = dir;
        transform.localScale = new Vector3(direction, 1, 1);
    }
}