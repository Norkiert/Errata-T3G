using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearedDoor : Gear
{
    [SerializeField, Range(0f, 0.001f)] protected float forceMultiplier;
    [SerializeField] protected bool reversed;

    [SerializeField] protected Transform maxHeightPoint;
        public Vector3 max;
    [SerializeField] protected Transform minHeightPoint;
        protected Vector3 min;

    protected void Awake()
    {
        min = minHeightPoint.position;
        max = maxHeightPoint.position;
    }
    public override void Rotate(float angle, Gear caller)
    {
        Rotate(angle * CachedRatio(caller));
    }
    public override void Rotate(float angle)
    {
        MyTransform.position += forceMultiplier * (reversed ? -1 : 1) * angle * Vector3.up;

        if(MyTransform.position.y > max.y)
        {
            MyTransform.position = max;
        }
        else if(MyTransform.position.y < min.y)
        {
            MyTransform.position = min;
        }
    }
}
