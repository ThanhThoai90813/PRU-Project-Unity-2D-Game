using System.Collections;
using UnityEngine;

public class PlaySoundInArea : MonoBehaviour
{
    // Tham chiếu đến AudioSource để phát âm thanh
    private AudioSource audioSource;

    // Biến để kiểm tra xem âm thanh đã được phát chưa (tránh lặp lại)
    private bool isPlaying = false;

    void Start()
    {
        // Lấy AudioSource từ GameObject (nếu không có thì thêm mới)
        audioSource = GetComponent<AudioSource>();

        // Nếu chưa có AudioSource, thêm vào và cấu hình mặc định
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("Không tìm thấy AudioSource, đã thêm mới một AudioSource.");
        }

        // Đảm bảo âm thanh không tự động phát khi bắt đầu
        audioSource.playOnAwake = false;
    }

    // Khi người chơi đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlaying)
        {
            // Phát âm thanh
            audioSource.Play();
            isPlaying = true;
        }
    }

    // Khi người chơi rời khỏi vùng trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAudio());
            isPlaying = false;
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 1f; // Fade trong 1 giây
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume về ban đầu
    }
}