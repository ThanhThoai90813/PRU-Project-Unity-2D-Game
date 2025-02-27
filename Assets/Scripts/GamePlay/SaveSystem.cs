using Inventory.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + "/save_slot_" + slot + ".json";
    }

    public static void SavePlayerData(Vector3 position, int slot, int playerHealth
        , List<InventoryItem> inventoryItems)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameData data = new GameData(sceneName, position, playerHealth, inventoryItems);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slot), json);
        Debug.Log("Saved position and heal to slot " + slot);
    }
    
    //public static (Vector3?, int?, List<InventoryItem>) LoadPlayerData(int slot)
    //{
    //    string path = GetSavePath(slot);
    //    if (File.Exists(path))
    //    {
    //        string json = File.ReadAllText(path);
    //        GameData data = JsonUtility.FromJson<GameData>(json);
    //        return (new Vector3(data.posX, data.posY, 0), data.playerHealth, data.inventoryItems);
    //    }
    //    Debug.LogWarning("No save data found in slot " + slot);
    //    return (null, null, new List<InventoryItem>());
    //}
    
    public static GameData LoadPlayerData(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }
        Debug.LogWarning("No save data found in slot " + slot);
        return null;
    }
}
