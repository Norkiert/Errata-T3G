using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ2 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    [SerializeField] protected Portals.Portal bluePortal;
    [SerializeField] protected Transform bluePortalStartingPosition;
    [SerializeField] protected Transform bluePortalCompletePosition;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();

        bluePortal.transform.position = bluePortalStartingPosition.position;
    }
    protected void Update()
    {
        if(!qGeneral.completed && erasers.Count == 0)
        {
            OnCompletion();
        }
    }
    protected void OnCompletion()
    {
        qGeneral.OnCompletion();

        bluePortal.transform.position = bluePortalCompletePosition.position;
    }

    public override bool QualifyImpulse(Impulse impulse)
    {
        if(erasers.Contains(impulse.track))
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
