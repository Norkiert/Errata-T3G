using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWeirdTPHendler : MonoBehaviour
{
    [SerializeField] private Transform tpPOint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.SetPosition(tpPOint.position);
        }
    }
}
