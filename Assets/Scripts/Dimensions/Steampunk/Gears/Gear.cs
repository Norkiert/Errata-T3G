using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class Gear : OptimizedMonoBehaviour
{
    [SerializeField] public float radius;

    public float RealRadius
    {
        get
        {
            return radius * MyTransform.lossyScale.y;
        }
    }

    [SerializeField] public List<Gear> connectedGears;
    [SerializeField] public List<Gear> coaxialGears;

    private Dictionary<Gear, float> cachedRatios = new Dictionary<Gear, float>();

    public virtual void Rotate(float angle, Gear caller)
    {
        float ratio = CachedRatio(caller);
        Rotate(-angle * ratio);
    }
    public virtual void Rotate(float angle)
    {
        MyTransform.Rotate(Vector3.forward, angle);
        foreach (var gear in connectedGears)
        {
            gear.Rotate(angle, this);
        }
        foreach (var gear in coaxialGears)
        {
            gear.Rotate(angle);
        }
    }
    protected float CachedRatio(Gear other)
    {
        if (cachedRatios.ContainsKey(other))
        {
            return cachedRatios[other];
        }
        else
        {
            return cachedRatios[other] = other.RealRadius / RealRadius;
        }
    }
}
