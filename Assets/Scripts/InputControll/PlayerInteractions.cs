using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float selectRange = 10f;
    [SerializeField] private LayerMask selectLayer;

    [SerializeField] [ReadOnly] private Interactable selectedObject = null;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateSelectedObject();
    }


    private void UpdateSelectedObject()
    {
        Interactable foundInteractable = null;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, selectRange, selectLayer))
            foundInteractable = hit.collider.GetComponent<Interactable>();

        if (selectedObject == foundInteractable)
            return;

        if (selectedObject)
            selectedObject.Select(false);

        if (foundInteractable)
            foundInteractable.Select(true);

        selectedObject = foundInteractable;
    }


}
