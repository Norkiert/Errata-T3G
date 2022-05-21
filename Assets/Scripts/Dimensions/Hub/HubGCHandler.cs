using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using GuideCube;

public class HubGCHandler : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool isGCubeInHub;

    public bool IsGCubeInHub => isGCubeInHub;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GCubeController>() != null)
            isGCubeInHub = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GCubeController>() != null)
            isGCubeInHub = false;
    }
}
