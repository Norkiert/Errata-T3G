using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearedDoor : Gear
{
    [SerializeField, Range(0f, 0.001f)] protected float forceMultiplier;
    [SerializeField] protected bool reversed;

    [SerializeField] protected Transform maxHeightPoint;
    [SerializeField] protected Transform minHeightPoint;

    protected Vector3 max;
    protected Vector3 min;

    protected void Awake()
    {
        min = minHeightPoint.position;
        max = maxHeightPoint.position;
    }
    public override void Rotate(float angle, Gear caller)
    {
        Rotate(angle * caller.RealRadius / radius);
    }
    public override void Rotate(float angle)
    {
        Transform t = transform;

        t.position += forceMultiplier * (reversed ? -1 : 1) * angle * Vector3.up;

        if(t.position.y > max.y)
        {
            t.position = max;
        }
        else if(t.position.y < min.y)
        {
            t.position = min;
        }
    }
}
