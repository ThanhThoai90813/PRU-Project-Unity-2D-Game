using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 10;
    private int direction = 1;
    public Vector2 knockback = Vector2.zero;
    public enum ArrowType { Straight, Arc, Wavy }
    public ArrowType arrowType = ArrowType.Straight; // Mặc định bay thẳng
    public float arcHeight = 2f; // Chiều cao đường vòng cung
    public float waveAmplitude = 0.5f; // Biên độ sóng
    public float waveFrequency = 2f; // Tần số sóng

    private Vector2 startPos;
    private float flightTime = 0f;

    void Start()
    {
        startPos = transform.position;
        if (lifetime <= 0)
        {
            lifetime = 2f; // Đảm bảo lifetime không bị đặt sai
        }
        Destroy(gameObject, lifetime);
    }


    //void Update()
    //{
    //    transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    //}
    void Update()
    {
        flightTime += Time.deltaTime;

        if (arrowType == ArrowType.Straight)
        {
            // Bay thẳng
            transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        }
        else if (arrowType == ArrowType.Arc)
        {
            // Bay theo vòng cung (parabol)
            float x = speed * direction * flightTime;
            float y = -4 * arcHeight * (x / speed) * (x / speed) + arcHeight;
            transform.position = new Vector2(startPos.x + x, startPos.y + y);
        }
        else if (arrowType == ArrowType.Wavy)
        {
            // Bay theo đường đánh võng (sinusoidal wave)
            float x = speed * direction * flightTime;
            float y = waveAmplitude * Mathf.Sin(waveFrequency * x);
            transform.position = new Vector2(startPos.x + x, startPos.y + y);
        }
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
        else if (!collision.CompareTag("Enemy")) // Không bị hủy khi chạm enemy
        {
            Destroy(gameObject);
        }
    }


    public void SetDirection(int dir)
    {
        direction = dir;
        if (direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

}
