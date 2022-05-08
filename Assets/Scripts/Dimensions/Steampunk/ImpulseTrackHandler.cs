using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImpulseTrackHandler : MonoBehaviour
{
    public abstract bool QualifyImpulse(Impulse impulse);
    public abstract void HandleImpulse(Impulse impulse);
}
