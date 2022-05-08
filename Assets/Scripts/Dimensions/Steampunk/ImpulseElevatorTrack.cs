using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ImpulseElevatorTrack : ImpulseTrackHandler
{
    [SerializeField, ReadOnly] protected ElevatorTrack track;

    [SerializeField] protected float timeToWait;
    
    protected void Awake()
    {
        track = GetComponent<ElevatorTrack>();
    }

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }

    public override void HandleImpulse(Impulse impulse)
    {
        StartCoroutine(WaitAndExtend());
    }

    protected IEnumerator WaitAndExtend()
    {
        float timeElapsed = 0f;
        for(; ; )
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToWait)
            {
                track.BeginExtension();
                yield break;
            }
            yield return null;
        }
    }
}
