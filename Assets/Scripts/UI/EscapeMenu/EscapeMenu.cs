using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagment;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Button resumeGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backToMenuButton;


    private void Start()
    {
        resumeGameButton.onClick.AddListener(ResumeGame);
        loadGameButton.onClick.AddListener(LoadGame);
        settingsButton.onClick.AddListener(OpenSettings);

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
        escMenu.SetActive(true);
        settingsPanel.SetActive(false);
    }
    private void Close()
    {
        escMenu.SetActive(false);
        settingsPanel.SetActive(false);
    }

    #region -Buttons functionality-

    private void ResumeGame()
    {
        Close();
        GameManager.ResumeGame();
    }
    private void LoadGame()
    {
        Debug.Log("ESC MENU: load game");
    }
    private void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


    #endregion
}
