using UnityEngine;
using TMPro; 

public class UINotification : MonoBehaviour
{
    [SerializeField]
    private GameObject notificationPanel; 

    [SerializeField]
    private TextMeshProUGUI notificationText; 

    [SerializeField]
    private float displayTime = 5f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip notificationSound;

    private void Awake()
    {
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
        var animEx = notificationPanel.GetComponent<Animation>();
        notificationPanel.transform.SetAsLastSibling();
        animEx.Play("Fade In1");
        notificationText.text = message;
        notificationPanel.SetActive(true);

        if (audioSource != null && notificationSound != null)
        {
            audioSource.PlayOneShot(notificationSound);
        }

        Invoke(nameof(HideNotification), displayTime);
    }

    private void HideNotification()
    {
        var animEx = notificationPanel.GetComponent<Animation>();
        animEx.Play("Fade out");
        notificationPanel.SetActive(false);
    }
}