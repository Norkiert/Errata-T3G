using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagment;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;



    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        loadButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        quitButton.onClick.AddListener(QuitGame);

        string path = Application.persistentDataPath + "/errata.json";
        if (!File.Exists(path))
            loadButton.interactable = false;
    }

    private void PlayGame()
    {
        HroberPrefsReset.ResetAll();
        GameManager.instance.LoadFirstGame();
    }

    private void LoadGame()
    {
        GameManager.instance.LoadGame();
    }

    private void OpenSettings()
    {
        CloseAllPanels();
        settingsMenu.SetActive(true);
    }

    private void OpenCredits()
    {
        CloseAllPanels();
        creditsMenu.SetActive(true);
    }

    private void QuitGame()
    {
        Debug.Log("Quit game");
#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void CloseAllPanels()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
}
