using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject SaveSlotMenuUI;
    public GameObject LoadSlotMenuUI;

    public Transform player;
    public int selectedSlot = 1; //defaut

    void Start()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(false);
        SaveSlotMenuUI.SetActive(false);
        LoadSlotMenuUI.SetActive(false);
        Time.timeScale = 1f; 
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
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
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
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ==== GỌI HÀM SAVE & LOAD ====
    public void SaveGame()
    {
        if (player != null)
        {
            SaveSystem.SavePosition(player.position, selectedSlot);
            Debug.Log("Saved to Slot " + selectedSlot);
        }
        CloseSlotMenus();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadGame()
    {
        Vector3? loadedPosition = SaveSystem.LoadPosition(selectedSlot);
        if (loadedPosition.HasValue && player != null)
        {
            player.position = loadedPosition.Value;
            Debug.Log("Loaded from Slot " + selectedSlot);
        }
        else
        {
            Debug.LogWarning("No save data found in Slot " + selectedSlot);
        }
        CloseSlotMenus();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSaveSlotMenu()
    {
        PauseMenuUI.SetActive(false);
        SaveSlotMenuUI.SetActive(true);
    }
    public void OpenLoadSlotMenu()
    {
        PauseMenuUI.SetActive(false);
        LoadSlotMenuUI.SetActive(true);
    }
    public void CloseSlotMenus()
    {
        SaveSlotMenuUI.SetActive(false);
        LoadSlotMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
    }
    public void SelectSlot(int slot)
    {
        selectedSlot = slot;
        Debug.Log("Selected Slot: " + slot);
    }

}
