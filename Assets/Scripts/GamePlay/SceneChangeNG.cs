using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeNG : MonoBehaviour
{
    public string sceneName;
    public Vector2 triggerPosition;

    void Start()
    {
        DBController.OverridePlayerPositionOnLoad = true;
        DBController.NewPlayerPosition = triggerPosition;

        if (LoadingScreenManager.Instance != null)
        {
            LoadingScreenManager.Instance.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("LoadingScreenManager không tồn tại! Đảm bảo rằng nó được khởi tạo trong scene.");
            // Fallback nếu không tìm thấy LoadingScreenManager
            SceneManager.LoadScene(sceneName);
        }
    }


}
