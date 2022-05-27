using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfGear : Gear
{
    [SerializeField] protected float angleLimitSmall;
    [SerializeField] protected float angleLimitBig;

    protected Dictionary<Gear, float> cachedAngles = new Dictionary<Gear, float>();

    protected float CachedAngle(Gear gear)
    {
        if (!cachedAngles.ContainsKey(gear))
        {
            float deltaX = MyTransform.position.z - gear.MyTransform.position.z;
            float deltaY = MyTransform.position.y - gear.MyTransform.position.y;

            cachedAngles[gear] = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
        }
        return cachedAngles[gear];
    }
    public override void Rotate(float angle)
    {
        MyTransform.Rotate(Vector3.forward, angle);
        foreach (var gear in connectedGears)
        {
            float angleToConnected = CachedAngle(gear);

            if (MyTransform.localEulerAngles.z % 360 >= (angleLimitSmall + angleToConnected + 90) % 360 && MyTransform.localEulerAngles.z % 360 <= (angleLimitBig + angleToConnected + 90) % 360)
            {
                gear.Rotate(angle, this);
            }
        }
        foreach (var gear in coaxialGears)
        {
            gear.Rotate(angle);
        }
    }
    public override void Rotate(float angle, Gear caller)
    {
        float ratio = CachedRatio(caller);
        Rotate(-angle * ratio);
    }
}
