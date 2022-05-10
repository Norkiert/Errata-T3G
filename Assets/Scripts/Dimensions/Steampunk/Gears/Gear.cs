using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class Gear : MonoBehaviour
{
    [SerializeField, ReadOnly] public const float Gear1_radius = .009f;
    [SerializeField, ReadOnly] public const float Gear2_radius = .007f;
    [SerializeField, ReadOnly] public const float Gear3_radius = .04f;
    [SerializeField, ReadOnly] public const float Gear4_radius = .04925f;
    [SerializeField, ReadOnly] public const float Gear5_radius = .017f;
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


    public void Rotate(float angle, Gear caller)
    {
        float ratio = caller.RealRadius / RealRadius;
        Rotate(-angle * ratio);
    }
    public void Rotate(float angle)
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
