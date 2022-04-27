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
        mirrorUI = FindObjectOfType<UIBehaviour>();
        playerController = FindObjectOfType<PlayerController>();
        player = playerController?.GetComponent<PlayerInteractions>();

        actualRotate = 0f;
    }
    
    public override void Select()
    {
        base.Select();
        ShowUI();
        player.OnInteractionStart += StartRotateMirror;
    }
    public override void Deselect()
    {
        base.Deselect();
        if (player)
        {
            HideUI();
            player.OnInteractionStart -= StartRotateMirror;
        }
    }

    private void StartRotateMirror()
    {
        playerController.FreezCamera = true;
        playerController.FreezMovement = true;
        StartCoroutine(RotateMirror());
    }
    private void EndRotateMirror()
    {
        playerController.FreezCamera = false;
        playerController.FreezMovement = false;
    }
    private IEnumerator RotateMirror()
    {
        HideUI();

        float stepCheckHorizontal = 0f;
        float stepCheckVertical = 0f;
        float reverseRotating = 1f;

        while (player.Interacting)
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");
            // Debug.Log(mx);

            if (LimitRotation)
            {
                if (   ((mx * reverseRotating > 0) && (actualRotate + mx * reverseRotating * rotationSpeed) > leftLimit)
                    || ((mx * reverseRotating < 0) && (actualRotate + mx * reverseRotating * rotationSpeed) < rightLimit))
                {
                    yield return null;
                    continue;
                }
            }

            if (rotateBySteps)
            {
                reverseRotating = (transform.position.x > player.transform.position.x) ? -1f : 1f;

                if (rotateHorizontal)
                {
                    if (mx > 0 && stepCheckHorizontal < 0)
                        stepCheckHorizontal = 0f + mx;
                    if (mx < 0 && stepCheckHorizontal > 0)
                        stepCheckHorizontal = 0f - mx;

                    if (stepCheckHorizontal > minMouseMoveToRotate || stepCheckHorizontal < -minMouseMoveToRotate)
                    {
                        transform.Rotate(new Vector3(0, (degreesPerStep * (stepCheckHorizontal < 0 ? -1 : 1)*reverseRotating), 0));
                        stepCheckHorizontal = 0f;
                    }
                    else
                        stepCheckHorizontal += mx;
                }

                if (rotateVertical)
                {
                    if (my > 0 && stepCheckVertical < 0)
                        stepCheckVertical = 0f + my;

                    if (my < 0 && stepCheckVertical > 0)
                        stepCheckVertical = 0f - my;

                    if (stepCheckVertical > minMouseMoveToRotate || stepCheckVertical < -minMouseMoveToRotate)
                    {
                        transform.Rotate(new Vector3(0, 0, (degreesPerStep * (stepCheckVertical < 0 ? -1 : 1) * reverseRotating)));
                        actualRotate += (degreesPerStep * (stepCheckVertical < 0 ? -1 : 1) * reverseRotating);
                        stepCheckVertical = 0f;
                    }
                    else
                        stepCheckVertical += my;
                }
            }
            else
            {
                reverseRotating = transform.position.x>player.transform.position.x ? -1f : 1f;
                transform.Rotate(new Vector3(0, (rotateHorizontal ? mx *reverseRotating : 0), (rotateVertical ? my : 0)) * rotationSpeed);
                actualRotate += mx * reverseRotating * rotationSpeed;
            }

            yield return null;
        }

        EndRotateMirror();
    }

    private void ShowUI() => mirrorUI.StartMirrorUI(rotateHorizontal, rotateVertical);
    private void HideUI() => mirrorUI.StopMirrorUI();
}
