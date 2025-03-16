using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    [SerializeField]
    private UINotification uiNotification; // Reference to the UINotification script

    [SerializeField]
    private string[] notificationMessages; // Array of possible messages to display

    [SerializeField]
    private int messageIndex = 0; // Index to select which message to show (can be set in the Inspector)

    private bool hasTriggered = false; // Biến kiểm tra để đảm bảo chỉ kích hoạt lần đầu

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player and hasn't triggered yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Đánh dấu là đã kích hoạt, không cho phép kích hoạt lại

            // Ensure the message index is within bounds
            if (notificationMessages == null || notificationMessages.Length == 0)
            {
                Debug.LogWarning("No notification messages set in the NotificationTrigger!");
                return;
            }

            messageIndex = Mathf.Clamp(messageIndex, 0, notificationMessages.Length - 1);
            string messageToShow = notificationMessages[messageIndex];

            // Show the notification using the UINotification script
            if (uiNotification != null)
            {
                uiNotification.ShowNotification(messageToShow);
            }
            else
            {
                Debug.LogError("UINotification reference is not assigned in NotificationTrigger!");
            }
        }
    }
}