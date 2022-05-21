using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HubPlayerHandler : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool isPlayerInHub;

    public bool IsPlayerInHub => isPlayerInHub;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            isPlayerInHub = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            isPlayerInHub = false;
    }
}
