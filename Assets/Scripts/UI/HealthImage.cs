using UnityEngine;

public class HealthSprite : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 0.5f, 0); // Tốc độ di chuyển (đơn vị world-space)
    public float timeToFade = 1f;

    private SpriteRenderer spriteRenderer;
    private float timeElapsed = 0f;
    private Color startColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on " + gameObject.name, gameObject);
            Destroy(gameObject);
            return;
        }

        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        // Di chuyển GameObject trong world-space
        transform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}