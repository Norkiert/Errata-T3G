using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IImpulseTrackHandler : MonoBehaviour
{
    public abstract bool QualifyImpulse(ImpulseTrack impulseTrack);
    public abstract void HandleImpulse(ImpulseTrack impulseTrack);
}
