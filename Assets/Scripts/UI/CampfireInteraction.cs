using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Press E to save game.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                DBController.Instance.PLAYER_POSITION = player.transform.position;
                DBController.Instance.CURRENTSCENE = SceneManager.GetActiveScene().name; // Gán tên scene hiện tại
                Debug.Log("Saving Player Position: " + DBController.Instance.PLAYER_POSITION);
                Debug.Log("Saving Scene: " + DBController.Instance.CURRENTSCENE); // Debug để kiểm tra
            }
            DBController.Instance.SaveNow();
            Debug.Log("Game Saved at Campfire!");
        }
    }
}
