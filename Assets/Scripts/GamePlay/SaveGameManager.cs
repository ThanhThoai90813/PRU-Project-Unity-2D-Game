using Inventory;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : Singleton<SaveGameManager>
{
    
    public Transform player;
    public InventoryController inventoryController;
    public int selectedSlot = 1; // Slot mặc định

    public void SelectSlot(int slot)
    {
        selectedSlot = slot;
        Debug.Log("Selected Slot: " + slot);
    }

    public void SaveGame()
    {
        if (player == null || inventoryController == null) return;
        
        Damageable damageable = player.GetComponent<Damageable>();
        int playerHealth = damageable.Health;
        List<InventoryItem> inventoryItems = new List<InventoryItem>(
            inventoryController.inventoryData.GetCurrentInventoryState().Values);

        SaveSystem.SavePlayerData(player.position, selectedSlot, playerHealth, inventoryItems);

        Debug.Log("Saved to Slot " + selectedSlot);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadPlayerData(selectedSlot);
        if (data != null)
        {
            if (SceneManager.GetActiveScene().name != data.sceneName)
            {
                StartCoroutine(LoadSceneAndApplyData(data)); // Load scene trước
            }
            else
            {
                ApplyLoadedData(data); // Nếu đang ở đúng scene, chỉ cần gán dữ liệu
            }
        }
        else
        {
            Debug.LogWarning("Load failed! No data found.");
        }
    }

    private IEnumerator LoadSceneAndApplyData(GameData data)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(data.sceneName);
        while (!asyncLoad.isDone) yield return null; // Chờ scene load xong

        yield return new WaitForSeconds(1f); // Chờ scene khởi tạo xong

        ApplyLoadedData(data);
    }

    private void ApplyLoadedData(GameData data)
    {
        player.position = new Vector3(data.posX, data.posY, 0);
        Damageable damageable = player.GetComponent<Damageable>();
        damageable.Health = data.playerHealth;
        //inventoryController.inventoryData.Initialize();

        //foreach (InventoryItem item in data.inventoryItems)
        //{
        //    inventoryController.inventoryData.AddItem(item);
        //}
        Debug.Log($"Loaded from Slot {selectedSlot} | Scene: {data.sceneName} | Position: {data.posX}, {data.posY} | Health: {data.playerHealth}");
    }

}
