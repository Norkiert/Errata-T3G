using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SplitterTrack : BasicTrack
{
    public new const float length = ModelTrack.length * 3;
    public new const float height = ModelTrack.height;
    public new const float width = length;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/SplitterTrack.prefab";

    [SerializeField] [ReadOnly] public bool hammerFacingRight = true;
    [SerializeField] public SplitterTrackMovingElement hammer;

    public override void RotateRight()
    {
        transform.Rotate(Vector3.up * 90);
        InitPos(position);
    }
    public override void RotateLeft()
    {
        transform.Rotate(Vector3.down * 90);
        InitPos(position);
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
        if (hammer.Rotating)
        {
            ball.ballRigidbody.velocity = transform.rotation * Vector3.forward * rollingSpeed * ball.rollingSpeed;
        }
        else
        {
            ball.ballRigidbody.velocity = rollingSpeed * ball.rollingSpeed * ball.ballRigidbody.velocity.normalized;
        }
    }
    public override void OnBallExit(BallBehavior ball)
    {

    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
        transform.localPosition += transform.localRotation * Vector3.forward * (ModelTrack.length - CurvedTrack.length - 0.03f);
    }
}
