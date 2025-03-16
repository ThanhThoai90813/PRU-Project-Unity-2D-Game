using UnityEngine;
using TMPro;
using System.Collections.Generic; // Để dùng List

public class TypewriterEffectMultiple : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> textComponents; // Danh sách các TextMeshProUGUI
    [SerializeField] private float delayBetweenChars = 0.05f; // Thời gian delay giữa các chữ
    [SerializeField] private float delayBetweenTexts = 1f; // Thời gian delay giữa các đoạn text

    private List<string> fullTexts = new List<string>(); // Lưu trữ toàn bộ text của từng đoạn
    private string currentText = ""; // Text hiện tại đang hiển thị
    private bool isTyping = false;

    void Start()
    {
        // Lưu trữ toàn bộ text ban đầu và xóa text trên UI để chuẩn bị cho hiệu ứng
        foreach (var textComponent in textComponents)
        {
            fullTexts.Add(textComponent.text);
            textComponent.text = "";
        }

        // Bắt đầu hiệu ứng
        StartTyping();
    }

    public void StartTyping()
    {
        if (!isTyping)
        {
            StartCoroutine(TypeTextSequentially());
        }
    }

    private System.Collections.IEnumerator TypeTextSequentially()
    {
        for (int i = 0; i < textComponents.Count; i++)
        {
            TextMeshProUGUI currentTextComponent = textComponents[i];
            string fullText = fullTexts[i];
            currentText = "";

            // Hiển thị từng chữ trong đoạn text hiện tại
            foreach (char letter in fullText.ToCharArray())
            {
                currentText += letter;
                currentTextComponent.text = currentText;
                yield return new WaitForSeconds(delayBetweenChars);
            }

            // Chờ một khoảng thời gian trước khi hiển thị đoạn text tiếp theo
            yield return new WaitForSeconds(delayBetweenTexts);
        }

        isTyping = false; // Kết thúc toàn bộ hiệu ứng
    }
}