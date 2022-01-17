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
    public override void Select()
    {
        base.Select();
        PlayerInteractions player = FindObjectOfType<PlayerInteractions>();
        player.OnInteractionStart += StartRotateMirror;
        player.OnInteractionEnd += EndRotateMirror;
    }
    public override void Deselect()
    {
        base.Deselect();
        PlayerInteractions player = FindObjectOfType<PlayerInteractions>();
        if(player)
        {
            player.OnInteractionStart -= StartRotateMirror;
            player.OnInteractionEnd -= EndRotateMirror;
        }
    }
    private void StartRotateMirror()
    {
        cController.enabled = false;
        StartCoroutine(RotateMirror());
    }
    private void EndRotateMirror()
    {
        cController.enabled = true;
    }
    private IEnumerator RotateMirror()
    {
        while(!cController.enabled)
        {
            yield return null;
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotationSpeed * Time.deltaTime);
        }
    }
}
