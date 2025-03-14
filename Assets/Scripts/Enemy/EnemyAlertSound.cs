using System.Collections;
using UnityEngine;

public class EnemyAlertSound : MonoBehaviour
{
    [SerializeField]
    private DetectionZone detectionZone; // Vùng phát hiện player

    [SerializeField]
    private AudioSource audioSource; // Component để phát âm thanh

    [SerializeField]
    private AudioClip alertSound; // Âm thanh khi phát hiện player

    private bool isPlayerInRange = false; // Theo dõi trạng thái player trong vùng

    void Update()
    {
        // Kiểm tra nếu có player trong vùng phát hiện
        if (detectionZone.detectedColliders.Count > 0)
        {
            if (!isPlayerInRange)
            {
                PlayAlertSound(); // Phát âm thanh khi player vào vùng
                isPlayerInRange = true;
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                StopAlertSound(); // Dừng âm thanh khi player rời vùng
                isPlayerInRange = false;
            }
        }
    }

    private void PlayAlertSound()
    {
        if (audioSource != null && alertSound != null)
        {
            audioSource.clip = alertSound; // Gán clip để có thể kiểm soát
            audioSource.Play(); // Phát âm thanh (có thể dừng sau này)
        }
        else
        {
            Debug.LogWarning("AudioSource hoặc AlertSound chưa được gán trong EnemyAlertSound!");
        }
    }

    private IEnumerator FadeOutSound(float fadeTime)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = startVolume; // Reset volume
        }
    }

    private void StopAlertSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            StartCoroutine(FadeOutSound(0.5f)); // Fade out trong 0.5 giây
        }
    }
}