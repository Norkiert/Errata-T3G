using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class YellowThingUnlocker : ImpulseTrackHandler
{
    [SerializeField] protected List<YellowThing> yellowThings;

    [SerializeField, ReadOnly] protected int ballCounter;
    [SerializeField] protected float waitTime;

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        ++ballCounter;

        if (ballCounter == 5)
        {
            ballCounter = 0;
            foreach (var yellowThing in yellowThings)
            {
                StartCoroutine(WaitNGO(yellowThing));
            }
        }
    }
    protected IEnumerator WaitNGO(YellowThing yellowThing)
    {
        //yield return new WaitForSeconds(waitTime);

        var mechanism = yellowThing.gearRigidbody.transform;
        float angleNow = mechanism.localEulerAngles.x;

        const float ratio = 177.267f / 62.515f;

        yellowThing.myRigidbody.useGravity = false;

        float deltaAngle = 0f;

        for (; ; )
        {
            yield return new WaitForFixedUpdate();

            if (deltaAngle == 0) deltaAngle = Mathf.Abs(mechanism.localEulerAngles.x - angleNow);

            else
            {
                yellowThing.myRigidbody.AddForceAtPosition(yellowThing.unlockForceMagnitude * -yellowThing.unlockForcePoint.forward, yellowThing.unlockForcePoint.position);

                if(yellowThing.MyTransform.localEulerAngles.x <= 0f)
                {
                    yellowThing.myRigidbody.useGravity = true;
                    yield break;
                }
            }

        }

        for (int i = 0; i < 8; i++)
        {
            yellowThing.myRigidbody.AddForceAtPosition(yellowThing.unlockForcePoint.forward * -yellowThing.unlockForceMagnitude, yellowThing.unlockForcePoint.position);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
       
        yield break;
    }
}
