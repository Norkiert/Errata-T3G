using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WeightedGear : Gear
{
    [SerializeField, Range(0, 1)] protected float forceMultiplier;
    [SerializeField] protected float maxForce;
    [SerializeField, ReadOnly] protected float force;

    [SerializeField] protected Transform pointLeft;
    [SerializeField] protected Transform pointRight;

    [SerializeField] protected LayerMask layer;

    [SerializeField, ReadOnly] protected bool stopFlag = false;

    protected void Update()
    {
        Rotate(force * Time.deltaTime);
        force *= forceMultiplier;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & layer.value) != 0)
        {
            if (Vector3.Distance(collision.gameObject.transform.position, pointLeft.transform.position) <= Vector3.Distance(collision.gameObject.transform.position, pointRight.transform.position))
            { // left
                StartCoroutine(RotateLeft());
            }
            else
            { // right
                StartCoroutine(RotateRight());
            }
        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        stopFlag = true;
    }
    protected IEnumerator RotateRight()
    {
        for(; ; )
        {
            if (stopFlag)
            {
                stopFlag = false;
                yield break;
            }
            force = maxForce;
            yield return null;
        }
    }
    protected IEnumerator RotateLeft()
    {
        for (; ; )
        {
            if (stopFlag)
            {
                stopFlag = false;
                yield break;
            }
            force = -maxForce;
            yield return null;
        }
    }
}
