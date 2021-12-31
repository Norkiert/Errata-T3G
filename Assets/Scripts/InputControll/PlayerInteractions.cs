using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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

    private void Update()
    {
        UpdateSelectedObject();

        UpdateHeldObject();
    }

    private void UpdateSelectedObject()
    {
        Interactable foundInteractable = null;
        if (Physics.SphereCast(playerCamera.position, 0.2f, playerCamera.forward, out RaycastHit hit, selectRange, selectLayer))
            foundInteractable = hit.collider.GetComponent<Interactable>();

        if (SelectedObject == foundInteractable)
            return;

        if (SelectedObject)
            SelectedObject.Select(false);

        if (foundInteractable)
            foundInteractable.Select(true);

        SelectedObject = foundInteractable;
    }

    private void UpdateHeldObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HeldObject)
                DropObject(HeldObject);
            else if (SelectedObject is Liftable liftable)
                PickUpObject(liftable);
        }

        if (HeldObject)
        {
            HeldObject.Rigidbody.velocity = (handTransform.position - HeldObject.transform.position) * holdingForce;
            HeldObject.transform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles + HeldObject.LiftDirectionOffset);
        }  
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
}
