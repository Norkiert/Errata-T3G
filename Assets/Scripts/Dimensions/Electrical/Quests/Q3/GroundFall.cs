using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    [SerializeField] private Connector cable;
    //[SerializeField] private Connector cable2;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("bul");
        if (other.gameObject.TryGetComponent<PlayerController>(out _))
        {
            cable.Disconnect(false);
            //cable2.Disconnect(false);
        }
    }
}
