using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Select")]
    [SerializeField] [Required] private Transform playerCamera;
    [SerializeField] private float selectRange = 10f;
    [SerializeField] private LayerMask selectLayer;
    [field: SerializeField] [field: ReadOnly] public Interactable SelectedObject { get; private set; } = null;

    [Header("Hold")]
    [SerializeField] [Required] private Transform handTransform;
    [SerializeField] [Min(1)] private float holdingForce = 10f;
    [SerializeField] private int heldObjectLayer;
    [field: SerializeField] [field: ReadOnly] public Liftable HeldObject { get; private set; } = null;

    [field: Header("Input")]
    [field: SerializeField] [field: ReadOnly] public bool Interacting { get; private set; } = false;

    public event Action OnInteractionStart;
    public event Action OnInteractionEnd;

    private void Start()
    {
        OnInteractionStart += ChangeHeldObject;
    }

    private void Update()
    {
        UpdateInput();

        UpdateSelectedObject();

        if (HeldObject)
            UpdateHeldObject();
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

    private void UpdateHeldObject()
    {
        HeldObject.Rigidbody.velocity = (handTransform.position - HeldObject.transform.position) * holdingForce;
        HeldObject.transform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles + HeldObject.LiftDirectionOffset);
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
        HeldObject = obj;
        obj.PickUp(heldObjectLayer);
    }
    private void DropObject(Liftable obj)
    {
        HeldObject = null;
        obj.Drop();
    }

    #endregion
}
