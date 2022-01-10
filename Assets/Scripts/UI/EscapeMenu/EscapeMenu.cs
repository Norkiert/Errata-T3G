using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Canvas escMenu;
    [SerializeField] private CameraController camController;
    private void Start()
    {
        Time.timeScale = 1f;
        GameObject temporaryPlayer = GameObject.Find("Player");
        escMenu.enabled = false;
        camController = temporaryPlayer.GetComponent<CameraController>();
        camController.FreezCamera = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale==0f? 1f:0f;
            escMenu.enabled = !escMenu.enabled;
            camController.FreezCamera = !camController.FreezCamera;

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
