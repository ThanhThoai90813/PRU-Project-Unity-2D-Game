using System;
using System.IO;
using System.Net.Security;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DBController : Singleton<DBController>
{
    private const string FILE_NAME_FORMAT = "Profile_{0}.txt";
    [SerializeField] private UserProfile _userProfile;
    
    private bool _pendingSave = false;
    private Task _currentSaveTask = null;
    private int _currentProfileIndex;
    
    //khai báo getter setter các biến cần lưu
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

    //chỗ xử lí việc save load
    protected override void CustomAwake()
    {
        _currentProfileIndex = 0;
        Initializing();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initializing();
    }

    private void Initializing()
    {
        ProfileData profileData = LoadData(_currentProfileIndex);
        _userProfile.SetProfileData(profileData);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = profileData.playerPosition;
            Debug.Log("Player position loaded: " + profileData.playerPosition);
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
                var jsonData = File.ReadAllText(path);
                return JsonUtility.FromJson<ProfileData>(jsonData);
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

        // Xóa dữ liệu trước đó
        ResetPlayerData();

        // Load dữ liệu mới
        ProfileData profileData = LoadData(_currentProfileIndex);
        _userProfile.SetProfileData(profileData);

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
        ForceSave();
        Debug.Log("SaveNow() called.");

    }

    public void SaveGame(int slot)
    {
        PlayerPrefs.SetInt($"PlayerHealth_Slot{slot}", PLAYERHEALTH);
        PlayerPrefs.Save();
        Debug.Log($"Game saved in slot {slot}");
    }
    public void ResetPlayerData()
    {
        _userProfile.SetProfileData(new ProfileData());
        PLAYER_POSITION = Vector2.zero;
    }

}


//chỗ khai báo biến cần lưu (chỉ nhận dữ liệu nguyên thủy)
[Serializable]
public class ProfileData
{
    public int health;
    public InventoryData inventoryData;
    public Vector2 playerPosition;
    
    public ProfileData()
    {
        health = 100;
        inventoryData = new InventoryData();
        playerPosition = Vector2.zero;
    }
}