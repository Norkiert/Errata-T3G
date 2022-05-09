using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[RequireComponent(typeof(BasicTrack))]
public class ImpulseTrack : MonoBehaviour
{
    [SerializeField] public List<ImpulseTrackHandler> handlers;

    [HideInInspector] public bool impulseMode = true;
    [ShowIf("impulseMode"), Button("Mode: On Enter")] protected void OnEnter()
    {
        impulseMode = false;
    }
    [HideIf("impulseMode"), Button("Mode: On Exit")] protected void OnExit()
    {
        impulseMode = true;
    }

    [SerializeField, HideInInspector] public BasicTrack track;

    protected void Awake()
    {
        track = GetComponent<BasicTrack>();
    }

    public void RegisterImpulse(BallBehavior ball)
    {
        track.balls.Add(ball);

        Impulse impulse = new Impulse(this, ball);

        foreach(var handler in handlers)
        {
            if (handler.QualifyImpulse(impulse))
            {
                handler.HandleImpulse(impulse);
            }
        }
    }
}

public struct Impulse
{
    public ImpulseTrack track;
    public BallBehavior ball;

    public Impulse(ImpulseTrack track, BallBehavior ball)
    {
        this.track = track;
        this.ball = ball;
    }
}