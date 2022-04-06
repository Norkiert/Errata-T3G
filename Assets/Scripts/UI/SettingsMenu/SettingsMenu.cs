using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider generalVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string mouseSensitivityKey = "mouseSensitivity";
    private const string fullScreenKey = "fullScreen";
    private const string qualityKey = "quality";

    private void Start()
    {
        LoadMouseSensitivity();
        LoadFullScreen();
        LoadQualityLevel();

        generalVolumeSlider.value = AudioManager.GeneralVolume;
        musicVolumeSlider.value = AudioManager.MusicVolume;
        sfxVolumeSlider.value = AudioManager.SFXVolume;
    }

    private void LoadMouseSensitivity()
    {
        if (playerController != null)
        {
            playerController.mouseSensitivity = mouseSensitivitySlider.value;
        }
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(mouseSensitivityKey, 8.0f);
    }

    public void SaveMouseSensitivity()
    {
        if (playerController != null)
            playerController.mouseSensitivity = mouseSensitivitySlider.value;

        PlayerPrefs.SetFloat(mouseSensitivityKey, mouseSensitivitySlider.value);
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        PlayerPrefs.SetInt(fullScreenKey, fullScreenToggle.isOn ? 1 : 0);
    }

    private void LoadFullScreen()
    {
        bool isFullScreen = PlayerPrefs.GetInt(fullScreenKey) == 1;
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
