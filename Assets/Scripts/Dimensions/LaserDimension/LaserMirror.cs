using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
public class LaserMirror : Interactable
{
    [Header("Rotation types")]
    [SerializeField] private bool rotateHorizontal = true;
    [SerializeField] private bool rotateVertical = false;
    [Header("Rotation options")]
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private bool rotateBySteps = false;
    [SerializeField, ShowIf(nameof(rotateBySteps))] float degreesPerStep = 30f;
    [SerializeField, ShowIf(nameof(rotateBySteps))] float minMouseMoveToRotate = 8f;
    [Header ("Rotation limits")]
    [SerializeField] private bool LimitRotation = false;
    [Header("Right Rotation limit need to be negative!"), ShowIf(nameof(LimitRotation))]
    [SerializeField] float rightLimit = -60f;
    [SerializeField, ShowIf(nameof(LimitRotation))] float leftLimit = 60f;

    private PlayerInteractions player;
    private PlayerController playerController;
    private UIBehaviour mirrorUI;
    private float actualRotate;

    private void Start()
    {
        if(FindObjectOfType<UIBehaviour>()!=null)
            mirrorUI = GameObject.Find("MirrorTipUI").GetComponent<UIBehaviour>();
        playerController = FindObjectOfType<PlayerController>();
        player = FindObjectOfType<PlayerInteractions>();
        actualRotate = 0f;
    }
    public override void Select()
    {
        base.Select();
        ShowUI();
        player.OnInteractionStart += StartRotateMirror;
        player.OnInteractionEnd += EndRotateMirror;
    }
    public override void Deselect()
    {
        base.Deselect();
        if (player)
        {
            HideUI();
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
        mirrorUI.StopMirrorUI();
        bool bul = true;
        float stepCheckHorizontal = 0f;
        float stepCheckVertical = 0f;
        float reverseRotating = 1f;
        while(!playerController.enabled)
        {
           // Debug.Log(Input.GetAxis("Mouse X"));
            if(LimitRotation)
            {
                if (((Input.GetAxis("Mouse X") * reverseRotating > 0) && (actualRotate + Input.GetAxis("Mouse X") * reverseRotating * rotationSpeed * Time.deltaTime) > leftLimit) || ((Input.GetAxis("Mouse X") * reverseRotating < 0) && (actualRotate + Input.GetAxis("Mouse X") * reverseRotating * rotationSpeed * Time.deltaTime) < rightLimit))
                {
                    bul = false;
                }
                else
                    bul = true;
            }
            if (rotateBySteps&&bul)
            {
                reverseRotating = (transform.position.x > player.transform.position.x) ? -1f : 1f;
                if (rotateHorizontal)
                {
                    if(Input.GetAxis("Mouse X")>0&&stepCheckHorizontal<0)
                        stepCheckHorizontal = 0f + Input.GetAxis("Mouse X");
                    if (Input.GetAxis("Mouse X") < 0 && stepCheckHorizontal > 0)
                        stepCheckHorizontal = 0f - Input.GetAxis("Mouse X");

                    if (stepCheckHorizontal > minMouseMoveToRotate || stepCheckHorizontal < -minMouseMoveToRotate)
                    {
                        transform.Rotate(new Vector3(0, (degreesPerStep * (stepCheckHorizontal < 0 ? -1 : 1)*reverseRotating), 0));
                        stepCheckHorizontal = 0f;
                    }
                    else
                        stepCheckHorizontal += Input.GetAxis("Mouse X");
                }
                if(rotateVertical)
                {
                    if (Input.GetAxis("Mouse Y") > 0 && stepCheckVertical < 0)
                        stepCheckVertical = 0f + Input.GetAxis("Mouse Y");
                    if (Input.GetAxis("Mouse Y") < 0 && stepCheckVertical > 0)
                        stepCheckVertical = 0f - Input.GetAxis("Mouse Y");

                    if (stepCheckVertical > minMouseMoveToRotate || stepCheckVertical < -minMouseMoveToRotate)
                    {
                        transform.Rotate(new Vector3(0, 0, (degreesPerStep * (stepCheckVertical < 0 ? -1 : 1) * reverseRotating)));
                        actualRotate += (degreesPerStep * (stepCheckVertical < 0 ? -1 : 1) * reverseRotating);
                        stepCheckVertical = 0f;
                    }
                    else
                        stepCheckVertical += Input.GetAxis("Mouse Y");
                }
            }
            else if(bul)
            {
                reverseRotating = (transform.position.x>player.transform.position.x)?-1f:1f;
                transform.Rotate(new Vector3(0, rotateHorizontal?Input.GetAxis("Mouse X")*reverseRotating:0, rotateVertical?Input.GetAxis("Mouse Y"):0) * rotationSpeed * Time.deltaTime);
                actualRotate += Input.GetAxis("Mouse X") * reverseRotating*rotationSpeed * Time.deltaTime;
            }
            yield return null;
        }
    }
    private void ShowUI()
    {
        mirrorUI.StartMirrorUI(rotateHorizontal,rotateVertical);
    }
    private void HideUI()
    {
        mirrorUI.StopMirrorUI();
    }
}
