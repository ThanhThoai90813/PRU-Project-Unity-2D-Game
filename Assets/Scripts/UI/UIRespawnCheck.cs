using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRespawnCheck : MonoBehaviour
{
    public GameObject respawnCheckPanel; 
    public Button yesButton;
    public Button noButton;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = respawnCheckPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = respawnCheckPanel.AddComponent<CanvasGroup>();
        }
        respawnCheckPanel.SetActive(false);
        canvasGroup.alpha = 0;
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }
    public void ShowCheckPanel()
    {
        respawnCheckPanel.SetActive(true);
        StartCoroutine(FadeInPanel());
        Time.timeScale = 0f;
    }

    private void OnYesClicked()
    {
        Time.timeScale = 1f;
        DBController.Instance.LoadGame(); 
    }
    private void OnNoClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator FadeInPanel()
    {
        float duration = 1f; 
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1; 
    }
}
