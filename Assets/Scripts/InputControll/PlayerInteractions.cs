using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Select")]
    [SerializeField, Required] private Transform playerCamera;
    [SerializeField] private float selectRange = 10f;
    [SerializeField] private LayerMask selectLayer;
    [field: SerializeField, ReadOnly] public Interactable SelectedObject { get; private set; } = null;

    [Header("Hold")]
    [SerializeField, Required] private Transform handTransform;
    [SerializeField, Min(1)] private float holdingForce = 0.5f;
    [SerializeField] private int heldObjectLayer;
    [SerializeField] [Range(0f, 90f)] private float heldClamXRotation = 45f;
    [field: SerializeField, ReadOnly] public Liftable HeldObject { get; private set; } = null;

    [field: Header("Input")]
    [field: SerializeField, ReadOnly] public bool Interacting { get; private set; } = false;

    public event Action OnInteractionStart;
    public event Action OnInteractionEnd;

    private void OnEnable()
    {
        OnInteractionStart += ChangeHeldObject;
        OnInteractionStart += ClickSelected;

        PlayerController.OnPlayerEnterPortal += CheckHeldObjectOnTeleport;
    }
    private void OnDisable()
    {
        OnInteractionStart -= ChangeHeldObject;
        OnInteractionStart -= ClickSelected;

        PlayerController.OnPlayerEnterPortal -= CheckHeldObjectOnTeleport;
    }

    private void Update()
    {
        UpdateInput();

        UpdateSelectedObject();

        if (HeldObject)
            UpdateHeldObjectPosition();
    }

    #region -input-

    private void UpdateInput()
    {
        bool interacting = Input.GetMouseButton(0);
        if (interacting != Interacting)
        {
            if (interacting)
                OnInteractionStart?.Invoke();
            else
                OnInteractionEnd?.Invoke();
        }
        Interacting = interacting;
    }

    #endregion

    #region -selected object-

    private void UpdateSelectedObject()
    {
        Interactable foundInteractable = null;
        if (Physics.SphereCast(playerCamera.position, 0.2f, playerCamera.forward, out RaycastHit hit, selectRange, selectLayer))
            foundInteractable = hit.collider.GetComponent<Interactable>();

        if (SelectedObject == foundInteractable)
            return;

        if (SelectedObject)
            SelectedObject.Deselect();

        if (foundInteractable)
            foundInteractable.Select();

        SelectedObject = foundInteractable;
    }

    #endregion

    #region -held object-

    private void UpdateHeldObjectPosition()
    {
        HeldObject.Rigidbody.velocity = (handTransform.position - HeldObject.transform.position) * holdingForce;

        Vector3 handRot = handTransform.rotation.eulerAngles;
        if (handRot.x > 180f)
            handRot.x -= 360f;
        handRot.x = Mathf.Clamp(handRot.x, -heldClamXRotation, heldClamXRotation);
        HeldObject.transform.rotation = Quaternion.Euler(handRot + HeldObject.LiftDirectionOffset);
    }
    private void ChangeHeldObject()
    {
        if (HeldObject)
            DropObject(HeldObject);
        else if (SelectedObject is Liftable liftable)
            PickUpObject(liftable);
    }
    private void PickUpObject(Liftable obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"{nameof(PlayerInteractions)}: Attempted to pick up null object!");
            return;
        }

        HeldObject = obj;
        obj.PickUp(heldObjectLayer);
    }
    private void DropObject(Liftable obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"{nameof(PlayerInteractions)}: Attempted to drop null object!");
            return;
        }

        HeldObject = null;
        obj.Drop();
    }

    private void CheckHeldObjectOnTeleport()
    {
        // TODO: check if can teleport with portal

        if (HeldObject != null)
            DropObject(HeldObject);
    }

    #endregion

    #region -clickables-


    protected void ClickSelected()
    {
        if (SelectedObject && SelectedObject is Clickable clickable)
        {
            clickable.Click();
        }
    }


    #endregion
}
