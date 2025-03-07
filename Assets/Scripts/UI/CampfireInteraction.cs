using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;
    [SerializeField]
    private GameObject saveMessageUI;

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
                DBController.Instance.CURRENTSCENE = SceneManager.GetActiveScene().name;
            }
            DBController.Instance.SaveNow();
            Debug.Log("Game Saved at Campfire!");
            StartCoroutine(ShowSaveMessage());
        }
    }
    private IEnumerator ShowSaveMessage()
    {
        if (saveMessageUI != null)
        {
            saveMessageUI.SetActive(true); 
            yield return new WaitForSeconds(2f); 
            saveMessageUI.SetActive(false); 
        }
    }

}
