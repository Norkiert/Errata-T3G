using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Canvas escMenu;

    private PlayerController playerController;

    private void Start()
    {
        Time.timeScale = 1f;
        escMenu.enabled = false;
        playerController = FindObjectOfType<PlayerController>();
        playerController.FreezCamera = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale==0f? 1f:0f;
            escMenu.enabled = !escMenu.enabled;
            playerController.FreezCamera = !playerController.FreezCamera;

            if (escMenu.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
