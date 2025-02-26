using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(false);
        Time.timeScale = 1f; // Đảm bảo game chạy bình thường
        isPaused = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }



    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
        isPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Dừng game
        isPaused = true;
    }

    public void OpenOptions()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Đảm bảo game chạy bình thường khi về menu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
