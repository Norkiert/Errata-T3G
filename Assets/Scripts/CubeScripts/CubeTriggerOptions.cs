using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTriggerOptions : MonoBehaviour
{
    private CubeController cube;
    [SerializeField] private Transform idlePoint;

    private void Start()
    {
        cube = FindObjectOfType<CubeController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerController _))
        {
            Vector3 targetPos = idlePoint.position;
            cube.SetNewIdlePoint(targetPos);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController _))
        {
            cube.StartFollowing();
        }
    }
}
