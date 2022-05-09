using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AutoSpawnerTrackImpulseLocker : ImpulseTrackHandler
{
    [SerializeField, ReadOnly] protected AutoSpawnerTrack autoSpawnerTrack;

    [SerializeField] public int ballsToLock = 0;
    [SerializeField, ReadOnly] protected int ballCounter = 0;

    protected void Awake()
    {
        autoSpawnerTrack = GetComponent<AutoSpawnerTrack>();
    }
    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        ++ballCounter;
        if(ballCounter >= ballsToLock)
        {
            ballCounter = 0;
            autoSpawnerTrack.locked = true;
            autoSpawnerTrack.timeElapsed = 0;
        }
    }
}
