using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class HubPlayerHandler : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool isPlayerInHub;

    public Action OnChange;

    public bool IsPlayerInHub => isPlayerInHub;

    private void OnEnable() => OnChange += ChangeState;
    private void OnDisable() => OnChange -= ChangeState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            isPlayerInHub = true;
            OnChange?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            isPlayerInHub = false;
            OnChange?.Invoke();
        }
    }

    private void ChangeState()
    {
        // debug
        if (isPlayerInHub)
            Debug.Log("Player enter hub");
        else
            Debug.Log("Player exit hub");

        // save game
        SaveManager.SaveGame();
    }
}
