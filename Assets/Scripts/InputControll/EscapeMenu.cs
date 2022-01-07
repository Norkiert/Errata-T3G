using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Canvas escMenu;
    [SerializeField] private Canvas crosshair;
    private CameraController camController;
    private void Start()
    {
        Time.timeScale = 1f;
        crosshair.enabled = true;
        escMenu.enabled = false;
        camController = GetComponent<CameraController>();
        camController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale==0f? 1f:0f;
            escMenu.enabled = !escMenu.enabled;
            crosshair.enabled = !crosshair.enabled;
            camController.enabled = !camController.enabled;

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
