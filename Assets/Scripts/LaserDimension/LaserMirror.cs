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
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
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
        playerController.enabled = false;
        StartCoroutine(RotateMirror());
    }
    private void EndRotateMirror()
    {
        playerController.enabled = true;
    }
    private IEnumerator RotateMirror()
    {
        float reverseRotating = 1f;
        while(!playerController.enabled)
        {
            reverseRotating = (transform.position.x>player.transform.position.x)?-1f:1f;
            yield return null;
            transform.Rotate(new Vector3(0, rotateHorizontal?Input.GetAxis("Mouse X")*reverseRotating:0, rotateVertical?Input.GetAxis("Mouse Y"):0) * rotationSpeed * Time.deltaTime);
        }
    }
}
