using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagment;

public class EscapeMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmExitPanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backToMenuButton;

    [Header("Other")]
    [SerializeField] private HubPlayerHandler playerHandler;

    private void Awake()
    {
        resumeGameButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        backToMenuButton.onClick.AddListener(TryBackToMenu);
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsGamePaused)
            {
                Close();
                GameManager.ResumeGame();
            }
            else
            {
                Open();
                GameManager.PauseGame();
            }
        }
    }

    private void Open()
    {
        CloseAllAditionalPanels();
        escMenu.SetActive(true);
    }
    private void Close()
    {
        CloseAllAditionalPanels();
        escMenu.SetActive(false);
    }

    private void CloseAllAditionalPanels()
    {
        settingsPanel.SetActive(false);
        confirmExitPanel.SetActive(false);
    }

    #region -Buttons functionality-

    private void ResumeGame()
    {
        Close();
        GameManager.ResumeGame();
    }
    private void OpenSettings()
    {
        CloseAllAditionalPanels();
        settingsPanel.SetActive(true);
    }
    private void TryBackToMenu()
    {
        CloseAllAditionalPanels();

        if (playerHandler == null || !playerHandler.IsPlayerInHub)
            confirmExitPanel.SetActive(true);
        else
            BackToMenu();
    }
    public void BackToMenu() => DimensionManager.BackToMenu();


    #endregion
}
