using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImpulseTrackHandler : MonoBehaviour
{
    public abstract bool QualifyImpulse(ImpulseTrack impulseTrack);
    public abstract void HandleImpulse(ImpulseTrack impulseTrack);
}
