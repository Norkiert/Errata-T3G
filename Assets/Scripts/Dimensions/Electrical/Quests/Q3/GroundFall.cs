using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    [SerializeField] private Connector cable;
    private void OnTriggerEnter(Collider other)
    {
            cable.Disconnect(false);
    }
}
