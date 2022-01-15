using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LaserMirror : Interactable
{
    [SerializeField] private KeyCode rotateKey = KeyCode.Mouse0;

    [SerializeField] private float rotationSpeed = 20;
    private CameraController cController;
    private void Start()
    {
        cController = FindObjectOfType<CameraController>();
    }
    private void Update()
    {
        if (IsSelected && Input.GetKey(rotateKey))
        {
            cController.enabled = false;
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotationSpeed * Time.deltaTime);
        }
        else cController.enabled = true;
    }
}
