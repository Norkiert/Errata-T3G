using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AutoSpawnerTrackImpulseUnlocker : ImpulseTrackHandler
{
    [SerializeField, ReadOnly] protected AutoSpawnerTrack autoSpawnerTrack;

    [SerializeField] protected AutoSpawnerTrackImpulseLocker entangledLocker;
    protected bool ShowBallsToUnlock() => entangledLocker;
    [SerializeField, HideIf("ShowBallsToUnlock")] protected int ballsToUnlock = 0;
    protected int BallsToUnlock
    {
        get
        {
            if (entangledLocker)
            {
                return entangledLocker.ballsToLock;
            }
            else
            {
                return ballsToUnlock;
            }
        }
    }
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
        if (ballCounter >= BallsToUnlock)
        {
            ballCounter = 0;
            autoSpawnerTrack.locked = false;
            autoSpawnerTrack.timeElapsed = 0;
        }
    }
}
