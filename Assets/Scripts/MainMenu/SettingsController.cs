using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SettingsController : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDrop;
    public Dropdown textQualityDrop;
    public Dropdown antialiasingDrop;
    public Dropdown vSyncDrop;
    public Slider volume;
    public Button saveButton;


    private Resolution[] resolutions;
    private Settings gameSettings;
    private string settingsFilePath;

    void OnEnable()
    {
        settingsFilePath = Application.persistentDataPath + "/gamesettings.json";
        gameSettings = new Settings();

        fullscreenToggle.onValueChanged.AddListener(delegate { FullscreenToggle(); });
        resolutionDrop.onValueChanged.AddListener(delegate { ResolutionChange(); });
        textQualityDrop.onValueChanged.AddListener(delegate { TextQChange(); });
        antialiasingDrop.onValueChanged.AddListener(delegate { AntialiasingChange(); });
        vSyncDrop.onValueChanged.AddListener(delegate { VsyncChange(); });
        volume.onValueChanged.AddListener(delegate { VolumeChange(); });
        saveButton.onClick.AddListener(delegate { SaveSettings(); });

        LoadResolutions();
        LoadSettings();
    }

    void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();

        foreach (Resolution res in resolutions)
        {
            resolutionDrop.options.Add(new Dropdown.OptionData(res.width + "x" + res.height + " " + res.refreshRate + "Hz"));
        }
    }

    public void FullscreenToggle()
    {
        gameSettings.fullscreen = fullscreenToggle.isOn;
        Screen.fullScreen = gameSettings.fullscreen;
    }

    public void ResolutionChange()
    {
        Resolution selectedRes = resolutions[resolutionDrop.value];
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen, selectedRes.refreshRate);
        gameSettings.resolutionIndex = resolutionDrop.value;
    }

    public void AntialiasingChange()
    {
        gameSettings.antialiasing = (int)Mathf.Pow(2, antialiasingDrop.value);
        QualitySettings.antiAliasing = gameSettings.antialiasing;
    }

    public void VsyncChange()
    {
        gameSettings.vSync = vSyncDrop.value;
        QualitySettings.vSyncCount = gameSettings.vSync;
    }

    public void TextQChange()
    {
        gameSettings.textureQuality = textQualityDrop.value;
        QualitySettings.globalTextureMipmapLimit = gameSettings.textureQuality;
    }

    public void VolumeChange()
    {
        gameSettings.volume = volume.value;
        AudioListener.volume = gameSettings.volume;
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(settingsFilePath, jsonData);
        MenuController.instance.closeOptions();
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string jsonData = File.ReadAllText(settingsFilePath);
            gameSettings = JsonUtility.FromJson<Settings>(jsonData);
        }
        else
        {
            gameSettings = new Settings();
        }

        fullscreenToggle.isOn = gameSettings.fullscreen;
        resolutionDrop.value = Mathf.Clamp(gameSettings.resolutionIndex, 0, resolutions.Length - 1);
        antialiasingDrop.value = Mathf.Clamp((int)Mathf.Log(gameSettings.antialiasing, 2), 0, 3);
        vSyncDrop.value = gameSettings.vSync;
        textQualityDrop.value = gameSettings.textureQuality;
        volume.value = gameSettings.volume;

        resolutionDrop.RefreshShownValue();
    }
}
