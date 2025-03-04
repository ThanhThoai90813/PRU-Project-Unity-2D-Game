using System;
using System.IO;
using UnityEngine;

public class DBController : Singleton<DBController>
{
    #region VARIABLE

    private int _coin;
    public int COIN
    {
        get => _coin;
        set
        {
            _coin = value;
            Save(DBKey.COIN, value);
        }
    }
    private int _playerHeal;
    public int PLAYERHEAL
    {
        get => _playerHeal;
        set
        {
            _playerHeal = value;
            Save(DBKey.PLAYERHEAL, value);
        }
    }
    private InventoryData _inventoryData;
    public InventoryData INVENTORY_DATA
    {
        get => _inventoryData;
        set
        {
            _inventoryData = value;
            Save(DBKey.INVENTORY_DATA, value);
        }
    }
    #endregion
    protected override void CustomAwake()
    {
        Initializing();
    }

    //Load data khi mới vÀo Scene mẹ
    void Initializing()
    {
        CheckDependency(DBKey.COIN, key => COIN = 0);
        CheckDependency(DBKey.PLAYERHEAL, key => PLAYERHEAL = 1000);
        CheckDependency(DBKey.INVENTORY_DATA, key =>
        {
            INVENTORY_DATA = new InventoryData();
        });

        Load();
    }

    void CheckDependency(string key, Action<string> onComplete)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            onComplete?.Invoke(key);
        }
    }

    void Save<T>(string key, T values)
    {
        if (typeof(T) == typeof(int) ||
            typeof(T) == typeof(bool) ||
            typeof(T) == typeof(string) ||
            typeof(T) == typeof(float) ||
            typeof(T) == typeof(long) ||
            typeof(T) == typeof(Quaternion) ||
            typeof(T) == typeof(Vector2) ||
            typeof(T) == typeof(Vector3) ||
            typeof(T) == typeof(Vector2Int) ||
            typeof(T) == typeof(Vector3Int))
        {
            PlayerPrefs.SetString(key, values.ToString());
            SaveAllToTextFile();
        }
        else
        {
            try
            {
                string json = JsonUtility.ToJson(values);
                PlayerPrefs.SetString(key, json);
                SaveAllToTextFile();
                Debug.Log("Saved InventoryData: " + json);
            }
            catch (UnityException e)
            {
                throw new UnityException(e.Message);
            }
        }
    }

    T LoadDataByKey<T>(string key)
    {
        if (typeof(T) == typeof(int) ||
            typeof(T) == typeof(bool) ||
            typeof(T) == typeof(string) ||
            typeof(T) == typeof(float) ||
            typeof(T) == typeof(long) ||
            typeof(T) == typeof(Quaternion) ||
            typeof(T) == typeof(Vector2) ||
            typeof(T) == typeof(Vector3) ||
            typeof(T) == typeof(Vector2Int) ||
            typeof(T) == typeof(Vector3Int))
        {
            string stringValue = PlayerPrefs.GetString(key);
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        else
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }
    }

    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    void Load()
    {
        _coin = LoadDataByKey<int>(DBKey.COIN);
        _playerHeal = LoadDataByKey<int>(DBKey.PLAYERHEAL);
        _inventoryData = LoadDataByKey<InventoryData>(DBKey.INVENTORY_DATA);
        Debug.Log("Loaded InventoryData: " + JsonUtility.ToJson(_inventoryData));
    }

    // New method to save all variables to a text file
    public void SaveAllToTextFile()
    {
        DBData dbData = new DBData
        {
            coin = _coin,
            playerHeal = _playerHeal,
            inventoryData = _inventoryData

        };

        string json = JsonUtility.ToJson(dbData);
        SaveToTextFile("DBData.txt", json);
    }

    public void SaveToTextFile(string fileName, string data)
    {
        string path = Path.Combine(Application.dataPath, "Resources", fileName);
        File.WriteAllText(path, data);
    }

    public string LoadFromTextFile(string fileName)
    {
        string path = Path.Combine(Application.dataPath, "Resources", fileName);
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("File not found: " + path);
            return null;
        }
    }
}

public static class DBKey
{
    public static readonly string COIN = "COIN";
    public static readonly string PLAYERHEAL = "PLAYERHEAL"; // định danh tên biến lưu
    public static readonly string INVENTORY_DATA = "INVENTORY_DATA"; // định danh inventory

}

[Serializable]
public class DBData
{
    public int coin;
    public int playerHeal;
    public InventoryData inventoryData;
}