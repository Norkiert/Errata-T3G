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
        var mechanism = yellowThing.gearRigidbody.transform;
        float angleNow = mechanism.localEulerAngles.x;

        yellowThing.myRigidbody.useGravity = false;

        float deltaAngle = 0f;

        float timeElapsed = 0;

        for (; ; )
        {
            yield return new WaitForFixedUpdate();

            timeElapsed += Time.fixedDeltaTime;

            if(timeElapsed >= 0.25)
            {
                yellowThing.myRigidbody.AddForceAtPosition(yellowThing.unlockForceMagnitude * -yellowThing.unlockForcePoint.forward, yellowThing.unlockForcePoint.position);
                if(timeElapsed >= 5)
                {
                    yellowThing.myRigidbody.useGravity = true;
                    yield break;
                }
            }

            /*if (deltaAngle == 0) deltaAngle = Mathf.Abs(mechanism.localEulerAngles.x - angleNow);

            else
            {
                yellowThing.myRigidbody.AddForceAtPosition(yellowThing.unlockForceMagnitude * -yellowThing.unlockForcePoint.forward, yellowThing.unlockForcePoint.position);

                Debug.Log(yellowThing.MyTransform.localEulerAngles.x);

                if(Mathf.Abs(yellowThing.MyTransform.localEulerAngles.x - 346) <= 1f)
                {
                    yellowThing.myRigidbody.useGravity = true;
                    yield break;
                }
            }

            angleNow = mechanism.localEulerAngles.x; */   

        }
    }
}
