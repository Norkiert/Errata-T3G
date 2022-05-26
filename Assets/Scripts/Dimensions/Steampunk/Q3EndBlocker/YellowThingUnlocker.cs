using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class YellowThingUnlocker : ImpulseTrackHandler
{
    [SerializeField] protected List<YellowThing> yellowThings;

    [SerializeField, ReadOnly] protected int ballCounter;

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
            foreach(var yellowThing in yellowThings)
            {
                yellowThing.myRigidbody.AddForceAtPosition(yellowThing.unlockForcePoint.forward * -yellowThing.unlockForceMagnitude, yellowThing.unlockForcePoint.position);
            }
        }
    }
}
