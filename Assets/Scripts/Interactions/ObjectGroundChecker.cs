using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectGroundChecker : OptimizedMonoBehaviour
{
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] [ReadOnly] public bool isGrounded;
    [SerializeField] public Transform groundTransform;

    protected void OnCollisionEnter(Collision collision)
    {
        if((1 << collision.gameObject.layer & groundLayer.value) != 0)
        {
            isGrounded = true;
            groundTransform = collision.transform;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer & groundLayer.value) != 0 && groundTransform == collision.transform)
        {
            isGrounded = false;
            groundTransform = null;
        }
    }
}
