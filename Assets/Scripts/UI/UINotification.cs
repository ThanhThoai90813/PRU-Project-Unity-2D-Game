using UnityEngine;
using TMPro; // Nếu dùng TextMeshPro
// using UnityEngine.UI; // Nếu dùng UI Text thông thường

public class UINotification : MonoBehaviour
{
    [SerializeField]
    private GameObject notificationPanel; // Gán NotificationPanel từ Inspector

    [SerializeField]
    private TextMeshProUGUI notificationText; // Gán NotificationText (TextMeshPro)
    // private Text notificationText; // Nếu dùng UI Text thay vì TextMeshPro

    [SerializeField]
    private float displayTime = 3f; // Thời gian hiển thị thông báo (giây)

    private void Awake()
    {
        // Đảm bảo panel tắt khi bắt đầu
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }

    public void ShowNotification(string message)
    {
        if (notificationPanel == null || notificationText == null)
        {
            Debug.LogError("Notification UI components chưa được gán!");
            return;
        }

        // Hiển thị panel và cập nhật nội dung
        notificationText.text = message;
        notificationPanel.SetActive(true);

        // Tự động tắt sau một khoảng thời gian
        Invoke(nameof(HideNotification), displayTime);
    }

    private void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}