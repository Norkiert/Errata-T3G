using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTrack : BasicTrack
{
    public new const float length = ModelTrack.length * 2;
    public new const float height = ModelTrack.height;
    public new const float width = length;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/CrossTrack.prefab";

    public override void RotateRight()
    {
        transform.Rotate(Vector3.up * 90);
        //InitPos(position);
    }
    public override void RotateLeft()
    {
        transform.Rotate(Vector3.down * 90);
        //InitPos(position);
    }
    public override void InitBallMovement(BallBehavior ball)
    {
        InitBallPath(ball);
        var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
        {
            0 => Vector3.forward,
            1 => Vector3.forward,
            2 => Vector3.back,
            3 => Vector3.back,
            _ => Vector3.zero
        });
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void MoveBall(BallBehavior ball)
    {
        ball.ballRigidbody.velocity = rollingSpeed * ball.rollingSpeed * ball.ballRigidbody.velocity.normalized;
    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
        transform.localPosition += transform.localRotation * (Vector3.forward + Vector3.right) * 0.5f;
    }
}
