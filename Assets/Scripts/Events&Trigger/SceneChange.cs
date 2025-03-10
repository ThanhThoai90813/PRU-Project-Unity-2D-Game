using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//xử lý trigger chuyển map và lưu lại dữ liệu người chơi sau khi chuyển
public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public GameObject confirmationPanel;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            if (player != null)
            {
                confirmationPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
    public void ConfirmSceneChange()
    {
        SaveAndChangeScene();
    }

    private void SaveAndChangeScene()
    {
        Time.timeScale = 1f;
        if (player != null)
        {
            Vector2 currentPosition = player.transform.position;
            currentPosition.y += 2f;
            DBController.Instance.PLAYER_POSITION = currentPosition;
            DBController.Instance.CURRENTSCENE = sceneName;
            DBController.Instance.SaveNow();
        }

        LoadingScreenManager.Instance.LoadScene(sceneName);
    }

    public void CancelSceneChange()
    {
        confirmationPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}