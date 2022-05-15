using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ3 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    [SerializeField] protected Portals.Portal purplePortal;
    [SerializeField] protected Transform purplePortalStartingPosition;
    [SerializeField] protected Transform purplePortalCompletePosition;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();

        purplePortal.transform.position = purplePortalStartingPosition.position;
    }
    protected void Update()
    {
        if (!qGeneral.completed && erasers.Count == 0)
        {
            OnCompletion();
        }
    }
    protected void OnCompletion()
    {
        qGeneral.OnCompletion();

        purplePortal.transform.position = purplePortalCompletePosition.position;
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
    }
}
