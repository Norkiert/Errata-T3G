using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField] private int sceneToLoad;

    private void Start()
    {
        playButton.onClick.AddListener(playGame);
        loadButton.onClick.AddListener(loadGame);
        settingsButton.onClick.AddListener(goToSettings);
        creditsButton.onClick.AddListener(goToCredits);
        quitButton.onClick.AddListener(quitGame);
    }

    void playGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void loadGame()
    {
        Debug.Log("Load");
    }

    void goToSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void goToCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    void quitGame()
    {
#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
