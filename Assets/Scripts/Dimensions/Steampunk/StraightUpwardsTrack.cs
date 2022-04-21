using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightUpwardsTrack : BasicTrack
{
    public new const float length = 0.5f * 2;
    public new const float height = 0.34f * 2;
    public new const float width = 0.24f * 2;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/StraightUpwardsTrack.prefab";
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
    public override void OnBallEnter(BallBehavior ball)
    {
        ball.pathID = 0;
        var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
        {
            0 => Vector3.forward,
            _ => Vector3.zero
        });
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void OnBallStay(BallBehavior ball)
    {
        ball.ballRigidbody.velocity = ball.rollingSpeed * rollingSpeed * ball.ballRigidbody.velocity.normalized;
    }
    public override void OnBallExit(BallBehavior ball)
    {

    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
