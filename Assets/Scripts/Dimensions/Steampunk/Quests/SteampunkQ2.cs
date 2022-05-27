using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

public class SteampunkQ2 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    [SerializeField] protected Portals.Portal bluePortal;
    protected Transform bluePortalT;
    [SerializeField] protected Transform bluePortalStartingPosition;
    [SerializeField] protected Transform bluePortalCompletePosition;

    [SerializeField] protected Transform blocker;
    [SerializeField] protected float blockerSpeed;

    [Header("Dialogues")]
    [SerializeField] private GameObject dialogueZone;
    [SerializeField] private DialoguePlayByMethod dialogueOnEnd;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();

        bluePortalT = bluePortal.transform;

        bluePortalT.position = bluePortalStartingPosition.position;
    }
    public void OnCompletion()
    {
        qGeneral.OnCompletion();

        bluePortalT.position = bluePortalCompletePosition.position;

        StartCoroutine(OpenBlocker());

        dialogueZone.SetActive(false);
        dialogueOnEnd.PlayDialoge();
    }

    protected IEnumerator OpenBlocker()
    {
        for(; ; )
        {
            blocker.localEulerAngles += blockerSpeed * Time.deltaTime * Vector3.forward;
            if(blocker.localEulerAngles.z >= 90)
            {
                blocker.localEulerAngles = new Vector3(blocker.localEulerAngles.x, blocker.localEulerAngles.y, blocker.localEulerAngles.z);
                yield break;
            }
            yield return null;
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
