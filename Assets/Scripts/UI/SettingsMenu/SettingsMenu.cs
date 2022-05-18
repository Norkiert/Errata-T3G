using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio;

public class SettingsMenu : MonoBehaviour
{
    public static float MouseSensitivity { get; private set; } = 1;

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider generalVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string mouseSensitivityKey = "mouseSensitivity";
    private const string fullScreenKey = "fullScreen";
    private const string qualityKey = "quality";

    [RuntimeInitializeOnLoadMethod]
    private static void UpdateValues()
    {
        MouseSensitivity = PlayerPrefs.GetFloat(mouseSensitivityKey, 1.0f);
    }

    private void Start()
    {
        gameObject.SetActive(false);

        LoadMouseSensitivity();
        LoadFullScreen();
        LoadQualityLevel();

        generalVolumeSlider.value = AudioManager.GeneralVolume;
        musicVolumeSlider.value = AudioManager.MusicVolume;
        sfxVolumeSlider.value = AudioManager.SFXVolume;
    }

    private void LoadMouseSensitivity() => mouseSensitivitySlider.value = PlayerPrefs.GetFloat(mouseSensitivityKey, 1.0f);

    public void SaveMouseSensitivity()
    {
        PlayerPrefs.SetFloat(mouseSensitivityKey, mouseSensitivitySlider.value);
        UpdateValues();
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        PlayerPrefs.SetInt(fullScreenKey, fullScreenToggle.isOn ? 1 : 0);
    }

    private void LoadFullScreen()
    {
        bool isFullScreen = PlayerPrefs.GetInt(fullScreenKey, 1) == 1;
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    }

    public void SetQualityLevel()
    {
        qualityDropdown.RefreshShownValue();
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        PlayerPrefs.SetInt(qualityKey, qualityDropdown.value);
    }

    private void LoadQualityLevel()
    {
        int qualityIndex = PlayerPrefs.GetInt(qualityKey, 2);
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
    }

    public void SetGeneralVolume() => AudioManager.SetGeneralVolume(generalVolumeSlider.value);
    public void SetMusicVolume() => AudioManager.SetMusicVolume(musicVolumeSlider.value);
    public void SetSFXVolume() => AudioManager.SetSFXVolume(sfxVolumeSlider.value);
}
