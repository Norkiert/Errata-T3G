using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableReset : MonoBehaviour
{
    [SerializeField] private GameObject CablesObject;
    [SerializeField] private GameObject ActualObject;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = ActualObject.transform.position;
    }
    public void RespawnCables()
    {
        Destroy(ActualObject);
        ActualObject = Instantiate(CablesObject);
        ActualObject.transform.position = startPosition;
    }
}
