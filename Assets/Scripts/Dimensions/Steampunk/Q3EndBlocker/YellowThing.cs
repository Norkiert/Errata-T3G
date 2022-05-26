using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowThing : ImpulseTrackHandler
{
    [SerializeField] protected Transform blockForcePoint;
    [SerializeField] protected float blockForceMagnitude;

    [SerializeField] public Transform unlockForcePoint;
    [SerializeField] public float unlockForceMagnitude;

    [SerializeField] public Rigidbody myRigidbody;

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        myRigidbody.AddForceAtPosition(blockForcePoint.up * -blockForceMagnitude, blockForcePoint.position);
    }
}
