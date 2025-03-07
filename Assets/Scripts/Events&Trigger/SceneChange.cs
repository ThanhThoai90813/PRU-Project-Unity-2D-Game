using UnityEngine;
using UnityEngine.SceneManagement;

//xử lý trigger chuyển map và lưu lại dữ liệu người chơi sau khi chuyển
public class SceneChanger : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            if (player != null)
            {
                Vector2 currentPosition = player.transform.position;
                DBController.Instance.PLAYER_POSITION = currentPosition;
                DBController.Instance.CURRENTSCENE = sceneName;
                DBController.Instance.SaveNow();
            }
            LoadingScreenManager.Instance.LoadScene(sceneName);
        }
    }
}