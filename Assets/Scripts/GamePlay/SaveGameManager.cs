using Inventory;
using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;
    
    public Transform player;
    public InventoryController inventoryController;
    public int selectedSlot = 1; // Slot mặc định

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
        if (player == null || inventoryController == null) return;

        var (position, health, inventoryItems) = SaveSystem.LoadPlayerData(selectedSlot);
        if (position.HasValue && health.HasValue)
        {
            player.position = position.Value;
            Damageable damageable = player.GetComponent<Damageable>();
            damageable.Health = health.Value;
            inventoryController.inventoryData.Initialize();

            foreach (InventoryItem item in inventoryItems)
            {
                inventoryController.inventoryData.AddItem(item);
            }
            Debug.Log($"Loaded from Slot {selectedSlot} | Position: {position.Value} | Health: {health.Value}");
        }
        else
        {
            Debug.LogWarning("Load failed! No data found.");
        }
    }

    public void SelectSlot(int slot)
    {
        selectedSlot = slot;
        Debug.Log("Selected Slot: " + slot);
    }
}
