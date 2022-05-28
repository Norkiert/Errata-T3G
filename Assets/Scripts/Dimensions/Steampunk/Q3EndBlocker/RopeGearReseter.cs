using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGearReseter : ImpulseTrackHandler
{
    [SerializeField] protected AutoGear autoGear;

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        StartCoroutine(RotateGear());
    }
    protected IEnumerator RotateGear()
    {
        float angle = 0;

        autoGear.enabled = true;

        float startingAngle = autoGear.MyTransform.localEulerAngles.z;
        float lastAngle = autoGear.MyTransform.localEulerAngles.z;
        for(; ; )
        {
            yield return null;
            angle += Mathf.Abs(lastAngle - autoGear.MyTransform.localEulerAngles.z);
            if (angle >= 360 * 2)
            {
                autoGear.Rotate(angle - 360);
                autoGear.enabled = false;
                autoGear.MyTransform.localEulerAngles = new Vector3(autoGear.MyTransform.localEulerAngles.x, autoGear.MyTransform.localEulerAngles.y, startingAngle);
                yield break;
            }
            lastAngle = autoGear.MyTransform.localEulerAngles.z;
        }
    }
}
