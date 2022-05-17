using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WeightedGear : Gear
{
    [SerializeField, Range(0, 1)] protected float forceMultiplier;
    [SerializeField, ReadOnly] protected float force;
    [SerializeField] protected float maxForce;

    [SerializeField, ReadOnly] protected bool stopFlag = false;

    [SerializeField] protected Transform pointLeft;
    [SerializeField] protected Transform pointRight;

    [SerializeField] protected LayerMask layer;

    protected void Update()
    {
        Rotate(force * Time.deltaTime);
        force *= forceMultiplier;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        var collisionGO = collision.gameObject;
        var collisionT = collision.transform;
        if ((1 << collisionGO.layer & layer.value) != 0)
        {
            if (Vector3.Distance(collisionT.position, pointLeft.position) <= Vector3.Distance(collisionT.position, pointRight.position))
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
    protected IEnumerator RotateRight() => RotateX(true);
    protected IEnumerator RotateLeft() => RotateX(false);
    protected IEnumerator RotateX(bool right)
    {
        for (; ; )
        {
            if (stopFlag)
            {
                stopFlag = false;
                yield break;
            }

            force = maxForce * (right ? 1 : -1);
            yield return null;
        }
    }
}
