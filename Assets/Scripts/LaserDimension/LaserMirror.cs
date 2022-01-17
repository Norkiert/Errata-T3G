using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LaserMirror : Interactable
{
    [SerializeField] private bool rotateHorizontal = true;
    [SerializeField] private bool rotateVertical = false;
    [SerializeField] private float rotationSpeed = 20;

    private PlayerInteractions player;
    private CameraController cController;

    private void Start()
    {
        cController = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerInteractions>();
    }
    public override void Select()
    {
        base.Select();
        player.OnInteractionStart += StartRotateMirror;
        player.OnInteractionEnd += EndRotateMirror;
    }
    public override void Deselect()
    {
        base.Deselect();
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
            transform.Rotate(new Vector3(0, rotateHorizontal?Input.GetAxis("Mouse X"):0, rotateVertical?Input.GetAxis("Mouse Y"):0) * rotationSpeed * Time.deltaTime);
        }
    }
}
