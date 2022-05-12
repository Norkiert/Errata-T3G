using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class Gear : MonoBehaviour
{
    [SerializeField] public float radius;
    public float RealRadius
    {
        get
        {
            return radius * transform.lossyScale.y;
        }
    }

    [SerializeField] public List<Gear> connectedGears;
    [SerializeField] public List<Gear> coaxialGears;


    public virtual void Rotate(float angle, Gear caller)
    {
        float ratio = caller.RealRadius / RealRadius;
        Rotate(-angle * ratio);
    }
    public virtual void Rotate(float angle)
    {
        transform.Rotate(Vector3.forward, angle);
        foreach (var gear in connectedGears)
        {
            gear.Rotate(angle, this);
        }
        foreach (var gear in coaxialGears)
        {
            gear.Rotate(angle);
        };
    }
}
