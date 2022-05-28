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

    [SerializeField] protected Rigidbody gearRigidbody;
    [SerializeField] protected Transform gearForcePoint;
    [SerializeField] protected float gearForceMagnitude;
    [SerializeField] protected float gearWaitTime;

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        myRigidbody.AddForceAtPosition(blockForcePoint.up * -blockForceMagnitude, blockForcePoint.position);

        StartCoroutine(DoGear());
    }
    protected IEnumerator DoGear()
    {
        yield return new WaitForSeconds(gearWaitTime);
        gearRigidbody.AddForceAtPosition(gearForcePoint.up * gearForceMagnitude, gearForcePoint.position);
        yield break;
    }
}
