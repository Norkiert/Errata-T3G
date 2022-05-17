using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImpulseTrackHandler : OptimizedMonoBehaviour
{
    public abstract bool QualifyImpulse(Impulse impulse);
    public abstract void HandleImpulse(Impulse impulse);
}
