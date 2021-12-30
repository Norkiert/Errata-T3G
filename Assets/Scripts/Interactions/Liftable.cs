using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Liftable : Interactable
{
    [field: SerializeField] public Vector3 LiftDirectionOffset { get; private set; } = Vector3.zero;

    public Rigidbody Rigidbody { get; protected set; }
    private int defaultLayer;

    protected override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody>();
        defaultLayer = gameObject.layer;
    }

    public void PickUp(int layer)
    {
        gameObject.layer = layer;
        Rigidbody.useGravity = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }
    public void Drop()
    {
        gameObject.layer = defaultLayer;
        Rigidbody.useGravity = true;
        Rigidbody.interpolation = RigidbodyInterpolation.None;
    }
}
