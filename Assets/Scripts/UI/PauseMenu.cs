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

    // ==== GỌI HÀM SAVE & LOAD ===================================================
    public void SaveGame()
    {
        Damageable damageable = player.GetComponent<Damageable>();
        int playerHealth = damageable.Health;
        SaveSystem.SavePlayerData(player.position, selectedSlot, playerHealth);
        Debug.Log("Saved to Slot " + selectedSlot);

        CloseSlotMenus();
        Time.timeScale = 1f;
        isPaused = false;
    }


    public void LoadGame()
    {

        var (position, health) = SaveSystem.LoadPlayerData(selectedSlot);
        player.position = position.Value;
        Damageable damageable = player.GetComponent<Damageable>();
        damageable.Health = health.Value;

        Debug.Log($"Loaded from Slot {selectedSlot} | Position: {position.Value} | Health: {health.Value}");

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
