using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

public class SteampunkQ3 : ImpulseTrackHandler
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected List<ImpulseTrack> erasers;

    [SerializeField] protected Dictionary<ImpulseTrack, float> erasersElapsedTime = new Dictionary<ImpulseTrack, float>();
    [SerializeField] protected float timeToWait;

    [SerializeField] protected Portals.Portal purplePortal;
    protected Transform purplePortalT;
    [SerializeField] protected Transform purplePortalStartingPosition;
    [SerializeField] protected Transform purplePortalCompletePosition;

    [Header("Dialogues")]
    [SerializeField] private GameObject dialogueZone;
    [SerializeField] private DialoguePlayByMethod dialogueOnEnd;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();

        purplePortalT = purplePortal.transform;

        purplePortalT.position = purplePortalStartingPosition.position;

        foreach(var eraser in erasers)
        {
            erasersElapsedTime.Add(eraser, -1);
        }
    }
    protected void Update()
    {
        if (!qGeneral.completed)
        {
            int counter = 0;
            foreach(var eraser in erasers)
            {
                if(erasersElapsedTime[eraser] != -1)
                {
                    erasersElapsedTime[eraser] += Time.deltaTime;

                    if(erasersElapsedTime[eraser] <= timeToWait)
                    {
                        ++counter;
                    }
                }
            }
            if(counter == erasers.Count)
            {
                OnCompletion();
            }
        }
    }
    public void OnCompletion()
    {
        qGeneral.OnCompletion();

        purplePortalT.position = purplePortalCompletePosition.position;

        dialogueZone.SetActive(false);
        dialogueOnEnd.PlayDialoge();
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
        erasersElapsedTime[impulse.track] = 0;
    }
}
