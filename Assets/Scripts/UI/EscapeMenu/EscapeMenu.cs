using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagment;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Canvas escMenu;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Button resumeGameButton;
    [SerializeField] private Button backToLobbyButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backToMenuButton;


    private void Start()
    {
        resumeGameButton.onClick.AddListener(ResumeGame);
        backToLobbyButton.onClick.AddListener(BackToLobby);
        loadGameButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(OpenSettings);
        backToMenuButton.onClick.AddListener(BackToMenu);
        escMenu.enabled = false;
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsGamePaused)
            {
                settingsPanel.SetActive(false);
                GameManager.ResumeGame();
                escMenu.enabled = false;
            }
            else
            {
                GameManager.PauseGame();
                escMenu.enabled = true;
            }
        }
    }
    #region -Buttons functionality-
    private void ResumeGame()
    {
        settingsPanel.SetActive(false);
        GameManager.ResumeGame();
        escMenu.enabled = false;
    }
    private void BackToLobby()
    {
        Debug.Log("ESC MENU: back to lobby");
    }
    void LoadGame()
    {
        Debug.Log("ESC MENU: load game");
    }
    private void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    private void BackToMenu()
    {
        Debug.Log("ESC MENU: Back to main menu");
    }
    #endregion
}
