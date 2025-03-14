using UnityEngine;

public class BushSwaying : MonoBehaviour
{
    [SerializeField]
    private float maxSwayAngle = 5f; // Góc nghiêng tối đa (độ)
    [SerializeField]
    private float swaySpeed = 0.2f; // Tốc độ gió (giá trị nhỏ để chậm hơn)
    [SerializeField]
    private float windStrength = 1f; // Độ mạnh của gió (có thể thay đổi theo thời gian)
    [SerializeField]
    private float noiseScale = 1f; // Độ chi tiết của Perlin Noise (để tạo sự tự nhiên)

    private float initialRotation; // Góc quay ban đầu
    private float noiseOffset; // Offset ngẫu nhiên để mỗi bụi cây chuyển động khác nhau

    private float baseSwayAngle;

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles.z;
        noiseOffset = Random.Range(0f, 100f);
        baseSwayAngle = maxSwayAngle; // Lưu giá trị gốc
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            maxSwayAngle = baseSwayAngle * 2f; // Tăng biên độ khi người chơi chạm
            Invoke(nameof(ResetSway), 1f); // Reset sau 1 giây
        }
    }

    private void ResetSway()
    {
        maxSwayAngle = baseSwayAngle; // Quay lại mức ban đầu
    }

    [SerializeField]
    private float windDirectionOffset = 2f; // Góc nghiêng thêm theo hướng gió

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * swaySpeed + noiseOffset, 0f) * 2f - 1f;
        float sway = noise * maxSwayAngle * windStrength;
        sway += windDirectionOffset; // Thêm hướng gió
        transform.rotation = Quaternion.Euler(0, 0, initialRotation + sway);
    }

}