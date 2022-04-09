using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class StraightTrack : BasicTrack
{
    public new const float length = 0.5f * 2;
    public new const float height = 0.12f * 2;
    public new const float width = 0.24f * 2;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/StraightTrack.prefab";
    protected new void Awake()
    {
        base.Awake();
    }
    public override void RotateRight()
    {
        transform.Rotate(Vector3.up * 90);
    }
    public override void RotateLeft()
    {
        transform.Rotate(Vector3.down * 90);
    }
    public override void MoveBall(BallBehavior ball)
    {
        if(ball.pathID == -1)
            InitBallPath(ball);
        var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
        {
            0 => Vector3.forward,
            1 => Vector3.back,
            _ => Vector3.zero
        });
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
