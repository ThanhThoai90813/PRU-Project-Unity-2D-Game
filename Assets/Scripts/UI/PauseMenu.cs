using Inventory;
using Inventory.Model;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject SaveSlotMenuUI;
    public GameObject LoadSlotMenuUI;

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
    /// <summary>
    /// /
    /// </summary>
    //public void SaveGame()
    //{
    //    SaveGameManager.Instance.SaveGame();
    //    CloseSlotMenus();
    //    Time.timeScale = 1f;
    //    isPaused = false;
    //}
    //public void LoadGame()
    //{
    //    SaveGameManager.Instance.LoadGame();
    //    CloseSlotMenus();
    //    Time.timeScale = 1f;
    //    isPaused = false;
    //}

    //public void OpenSaveSlotMenu()
    //{
    //    PauseMenuUI.SetActive(false);
    //    SaveSlotMenuUI.SetActive(true);
    //}
    //public void OpenLoadSlotMenu()
    //{
    //    PauseMenuUI.SetActive(false);
    //    LoadSlotMenuUI.SetActive(true);
    //}
    //public void CloseSlotMenus()
    //{
    //    SaveSlotMenuUI.SetActive(false);
    //    LoadSlotMenuUI.SetActive(false);
    //    PauseMenuUI.SetActive(true);
    //}
    //public void SelectSlot(int slot)
    //{
    //    SaveGameManager.Instance.SelectSlot(slot);
    //}

}
