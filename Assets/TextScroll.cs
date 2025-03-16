using UnityEngine;
using UnityEngine.UI; // Nếu bạn dùng Text hoặc TextMeshProUGUI

public class TextScroll : MonoBehaviour
{
    public float speed = 50f; // Tốc độ di chuyển (điều chỉnh theo ý muốn)
    private RectTransform rectTransform;

    void Start()
    {
        // Lấy component RectTransform của đối tượng văn bản
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Di chuyển văn bản lên trên theo thời gian
        rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // (Tùy chọn) Nếu muốn văn bản dừng lại hoặc biến mất sau khi di chuyển xong
        if (rectTransform.anchoredPosition.y > 10000) // Điều chỉnh giá trị 500 tùy theo nhu cầu
        {
            gameObject.SetActive(false); // Tắt văn bản khi nó đi quá cao
        }
    }
}