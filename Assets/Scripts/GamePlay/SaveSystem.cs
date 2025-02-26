using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + "/save_slot_" + slot + ".json";
    }

    public static void SavePosition(Vector3 position, int slot)
    {
        GameData data = new GameData(position);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slot), json);
        Debug.Log("Saved position to slot " + slot);
    }

    public static Vector3? LoadPosition(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return new Vector3(data.posX, data.posY, 0);
        }
        Debug.LogWarning("No save data found in slot " + slot);
        return null;
    }
}
