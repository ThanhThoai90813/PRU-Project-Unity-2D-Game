using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DBController : Singleton<DBController>
{
    private const string FILE_NAME_FORMAT = "Profile_{0}.txt";
    [SerializeField] private UserProfile _userProfile;
    private HashSet<int> collectedTokens = new HashSet<int>();

    private bool _pendingSave = false;
    private Task _currentSaveTask = null;
    private int _currentProfileIndex;

    //khai báo getter setter các biến cần lưu
    #region

    public string CURRENTSCENE
    {
        get => _userProfile.ProfileData.currentScene;
        set
        {
            _userProfile.ProfileData.currentScene = value;
            Debug.Log("Current scene set to: " + value);
        }
    }

    public int PLAYERHEALTH
    {
        get => _userProfile.ProfileData.health;
        set
        {
            _userProfile.ProfileData.health = value;
            //QueueSave(); //Gọi tự động lưu
        }
    }
    
    public InventoryData INVENTORY_DATA
    {
        get => _userProfile.ProfileData.inventoryData;
        set
        {
            _userProfile.ProfileData.inventoryData = value;
            //QueueSave(); //Gọi tự động lưu
        }
    }

    public Vector2 PLAYER_POSITION
    {
        get => _userProfile.ProfileData.playerPosition;
        set
        {
            _userProfile.ProfileData.playerPosition = value;
            //QueueSave(); //Gọi tự động lưu
        }
    }

    public string SAVEDATETIME
    {
        get => _userProfile.ProfileData.saveDateTime;
    }

    #endregion

    //chỗ xử lí việc save load
    protected override void CustomAwake()
    {
        _currentProfileIndex = 0;
        Initializing();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = _userProfile.ProfileData.playerPosition;
            Debug.Log("Player position restored in new scene: " + _userProfile.ProfileData.playerPosition);
        }
        //foreach (Item item in FindObjectsOfType<Item>())
        foreach (Item item in FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (IsItemCollected(item.GetToken()))
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void Initializing()
    {
        ProfileData profileData = LoadData(_currentProfileIndex);
        _userProfile.SetProfileData(profileData);

        collectedTokens = new HashSet<int>(profileData.collectedTokens);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = _userProfile.ProfileData.playerPosition;
            Debug.Log("Player position loaded: " + _userProfile.ProfileData.playerPosition);
        }
    }

    private void QueueSave()
    {
        if (!_pendingSave)
        {
            _pendingSave = true;
            
            // If we're not already saving, start saving immediately
            if (_currentSaveTask == null || _currentSaveTask.IsCompleted)
            {
                SaveAsync();
            }
        }
    }
    
    private async void SaveAsync()
    {
        // Wait a short frame to batch multiple rapid changes together
        await Task.Delay(50);
        
        try
        {
            _pendingSave = false;
            _userProfile.ProfileData.saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _userProfile.ProfileData.collectedTokens = new List<int>(collectedTokens);
            
            string jsonData = JsonUtility.ToJson(_userProfile.ProfileData);
            string FILE_NAME = string.Format(FILE_NAME_FORMAT, _currentProfileIndex.ToString());
            string path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            
            // Start the async file write operation
            _currentSaveTask = File.WriteAllTextAsync(path, jsonData);
            
            // Wait for it to complete
            await _currentSaveTask;
            
            // If more changes happened during saving, save again
            if (_pendingSave)
            {
                SaveAsync();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
            
            // If saving failed, try again later
            _pendingSave = true;
            await Task.Delay(1000); // Wait a bit longer before retry
            
            if (_pendingSave)
            {
                SaveAsync();
            }
        }
    }

    public ProfileData LoadData(int index)
    {
        try
        {
            string FILE_NAME = string.Format(FILE_NAME_FORMAT, index.ToString());
            string path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            if (File.Exists(path))
            {
                //var jsonData = File.ReadAllText(path);
                //return JsonUtility.FromJson<ProfileData>(jsonData);
               
                var jsonData = File.ReadAllText(path);
                ProfileData profileData = JsonUtility.FromJson<ProfileData>(jsonData);
                collectedTokens = new HashSet<int>(profileData.collectedTokens);
                return profileData;
            }
            Debug.Log("Database file not found - Creating new one");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data: {e.Message}");
        }
        
        return new ProfileData();
    }
    
    // Call this when application is quitting or pausing to ensure data is saved
    private void OnApplicationPause(bool pause)
    {
        if (pause && _pendingSave)
        {
            ForceSave();
        }
    }
    
    private void OnApplicationQuit()
    {
        if (_pendingSave)
        {
            ForceSave();
        }
    }
    
    // Synchronous save for critical moments (app closing, etc.)
    public void ForceSave()
    {
        try
        {
            _userProfile.ProfileData.saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _userProfile.ProfileData.collectedTokens = new List<int>(collectedTokens);
            string jsonData = JsonUtility.ToJson(_userProfile.ProfileData);
            string FILE_NAME = string.Format(FILE_NAME_FORMAT, _currentProfileIndex.ToString());
            string path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            File.WriteAllText(path, jsonData);
            _pendingSave = false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error force saving data: {e.Message}");
        }
    }
    
    public void ForceLoadCustomProfile(int index)
    {
        _currentProfileIndex = index;

        // Xóa dữ liệu trước đó để tránh bị override dũ liệu cũ
        ResetPlayerData();

        // Load dữ liệu mới
        ProfileData profileData = LoadData(_currentProfileIndex);
        _userProfile.SetProfileData(profileData);
        
        collectedTokens = new HashSet<int>(profileData.collectedTokens);

        //Cập nhật vị trí player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = profileData.playerPosition;
            Debug.Log("New Player Position Set: " + player.transform.position);
        }
    }
    
    // Optional: Public method to force a save manually
    public void SaveNow()
    {
        _userProfile.ProfileData.saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ForceSave();
        Debug.Log("SaveNow() called.");

    }
    public void NewGame()
    {   
        // Xóa file save hiện tại nếu tồn tại
        string fileName = string.Format(FILE_NAME_FORMAT, _currentProfileIndex.ToString());
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Đã xóa file save hiện tại: {fileName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Lỗi khi xóa file save: {e.Message}");
            }
        }

        // Reset dữ liệu và tạo profile mới
        _userProfile.SetProfileData(new ProfileData());
        PLAYER_POSITION = new Vector2(-46, -5);
        CURRENTSCENE = "MainMenu"; // Scene mặc định
        _userProfile.ProfileData.saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SaveNow();

        //LoadingScreenManager.Instance.LoadScene("NewGameCutScene");
        LoadingScreenManager.Instance.LoadScene("Map1_JungleMap");

    }

    public void SaveGame(int slot)
    {
        PlayerPrefs.SetInt($"PlayerHealth_Slot{slot}", PLAYERHEALTH);
        _userProfile.ProfileData.collectedTokens = new List<int>(collectedTokens);
        PlayerPrefs.Save();
        Debug.Log($"Game saved in slot {slot}");
    }

    public void LoadGame()
    {
        ProfileData profileData = LoadData(_currentProfileIndex);
        _userProfile.SetProfileData(profileData);

        LoadingScreenManager.Instance.LoadScene(profileData.currentScene);
    }

    public void ResetPlayerData()
    {
        _userProfile.SetProfileData(new ProfileData());
        PLAYER_POSITION = Vector2.zero;
    }

    public bool IsItemCollected(int itemToken)
    {
        return collectedTokens.Contains(itemToken);
    }
    public void AddCollectedToken(int itemToken)
    {
        if (!collectedTokens.Contains(itemToken))
        {
            collectedTokens.Add(itemToken);
            //_userProfile.ProfileData.collectedTokens.Add(itemToken);
            _userProfile.ProfileData.collectedTokens = new List<int>(collectedTokens); 
        }
    }

    public int GetCurrentProfileIndex()
    {
        return _currentProfileIndex;
    }
}


//chỗ khai báo thứ cần lưu (chỉ nhận dữ liệu nguyên thủy)
[Serializable]
public class ProfileData
{
    public int health;
    public InventoryData inventoryData;
    public Vector2 playerPosition;
    public string currentScene;
    public string saveDateTime;
    public List<string> collectedItems;
    public List<int> collectedTokens;
    public List<int> rewardedNPCs;
    public ProfileData()
    {
        health = 100;
        inventoryData = new InventoryData();
        playerPosition = new Vector2(-46, -5);
        currentScene = "MainMenu";
        saveDateTime = System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
        collectedItems = new List<string>();
        collectedTokens = new List<int>();
        rewardedNPCs = new List<int>();
    }
}