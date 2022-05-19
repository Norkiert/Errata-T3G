using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ4 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();
    }
    public void OnCompletion()
    {
        qGeneral.OnCompletion();
    }

    public override bool QualifyImpulse(Impulse impulse)
    {
        if (erasers.Contains(impulse.track))
        {
            return true;
        }
        return false;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        erasers.Remove(impulse.track);
        if (!qGeneral.completed && erasers.Count == 0)
        {
            OnCompletion();
        }
    }
}
