using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(BasicTrack))]
public class ImpulseTrack : MonoBehaviour
{
    [SerializeField] public List<ImpulseTrackHandler> handlers;

    [SerializeField, ReadOnly] public List<BallBehavior> balls;
    [SerializeField, ReadOnly] public BallBehavior lastBall;

    [SerializeField, ReadOnly] public BasicTrack track;

    protected void Awake()
    {
        track = GetComponent<BasicTrack>();
    }

    public void RegisterImpulse(BallBehavior ball)
    {
        balls.Add(ball);
        lastBall = ball;

        foreach(var handler in handlers)
        {
            if (handler.QualifyImpulse(this))
            {
                handler.HandleImpulse(this);
            }
        }
    }
}
