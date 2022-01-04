using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectGroundChecker : MonoBehaviour
{
    [SerializeField] protected LayerMask layer;
    [SerializeField] [ReadOnly] protected bool isGrounded;
    [SerializeField] protected Transform groundTransform;

    protected void OnCollisionEnter(Collision collision)
    {
        if((1 << collision.gameObject.layer & layer.value) != 0)
        {
            isGrounded = true;
            groundTransform = collision.transform;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer & layer.value) != 0)
        {
            isGrounded = false;
            groundTransform = null;
        }
    }
}
