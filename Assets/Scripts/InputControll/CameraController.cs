using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float sensitivity = 8f;
    [SerializeField] [Range(0, -90)] private float minXAngle = -90f;
    [SerializeField] [Range(0, 90)] private float maxXAngle = 90f;

    private float xRotation = 0f;

    public bool FreezCamera { get; set; } = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (FreezCamera)
            return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXAngle, maxXAngle);
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;

        transform.Rotate(Vector3.up, mouseX);
    }
}