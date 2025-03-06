using UnityEngine;
using UnityEngine.SceneManagement;

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
                DBController.Instance.CURRENTSCENENAME = sceneName;
                DBController.Instance.SaveNow();
                Debug.Log($"Saved: Scene = {sceneName}, Position = {currentPosition}");
            }
            // Chuyển scene
            SceneManager.LoadScene(sceneName);
        }
    }
}