using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ2 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    [SerializeField] protected Portals.Portal bluePortal;
    protected Transform bluePortalT;
    [SerializeField] protected Transform bluePortalStartingPosition;
    [SerializeField] protected Transform bluePortalCompletePosition;

    [SerializeField] protected List<GameObject> blockers;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();

        bluePortalT = bluePortal.transform;

        bluePortalT.position = bluePortalStartingPosition.position;
    }
    protected void OnCompletion()
    {
        qGeneral.OnCompletion();

        bluePortalT.position = bluePortalCompletePosition.position;

        foreach(var blocker in blockers)
        {
            blocker.SetActive(false);
        }
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
        if (!qGeneral.completed && erasers.Count == 0)
        {
            OnCompletion();
        }
    }
}
