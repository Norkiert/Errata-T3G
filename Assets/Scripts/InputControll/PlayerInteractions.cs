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
    [SerializeField] [ReadOnly] private Interactable selectedObject = null;

    [Header("Hold")]
    [SerializeField] [Required] private Transform handTransform;
    [SerializeField] [Min(1)] private float holdingForce = 10f;
    [SerializeField] private int heldObjectLayer;
    [SerializeField] [ReadOnly] private Liftable heldObject = null;

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

        if (selectedObject == foundInteractable)
            return;

        if (selectedObject)
            selectedObject.Select(false);

        if (foundInteractable)
            foundInteractable.Select(true);

        selectedObject = foundInteractable;
    }

    private void UpdateHeldObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject)
                DropObject(heldObject);
            else if (selectedObject is Liftable liftable)
                PickUpObject(liftable);
        }

        if (heldObject)
        {
            heldObject.Rigidbody.velocity = (handTransform.position - heldObject.transform.position) * holdingForce;
            heldObject.transform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles + heldObject.LiftDirectionOffset);
        }  
    }
    private void PickUpObject(Liftable obj)
    {
        heldObject = obj;
        obj.PickUp(heldObjectLayer);
    }
    private void DropObject(Liftable obj)
    {
        heldObject = null;
        obj.Drop();
    }
}
