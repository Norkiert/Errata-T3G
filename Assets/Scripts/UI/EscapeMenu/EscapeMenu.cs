using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Canvas escMenu;

    private void Start()
    {
        escMenu.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsGamePaused)
            {
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
    //Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
}
