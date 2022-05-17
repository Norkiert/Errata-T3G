using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerTrack : BasicTrack
{
    public new const float length = (ModelTrack.length + ModelTrack.width) / 2;
    public new const float width = ModelTrack.length;
    public new const float height = ModelTrack.height;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/MergerTrack.prefab";

    [SerializeField] public Transform rotationPoint_0;
    [SerializeField] public Transform rotationPoint_1;
    
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        MyTransform.localPosition = GetLocalPosition();
    }
    public override void RotateRight()
    {
        MyTransform.Rotate(Vector3.up * 90);
    }
    public override void RotateLeft()
    {
        MyTransform.Rotate(Vector3.down * 90);
    }
    public override void OnBallEnter(BallBehavior ball)
    {
        InitBallPath(ball);
        var rotationPoint = ball.pathID switch
        {
            0 => rotationPoint_0,
            1 => rotationPoint_1,
            _ => throw new System.Exception("Invalid BallBehavior's pathID.")
        };

        var deltaX = rotationPoint.position.x - ball.MyTransform.position.x;
        var deltaZ = rotationPoint.position.z - ball.MyTransform.position.z;

        var angle = Quaternion.Euler(0, Mathf.Atan2(deltaZ, deltaX) * Mathf.Rad2Deg, 0) * MyTransform.rotation;

        var moveVector = angle * (Quaternion.Inverse(MyTransform.rotation) * (ball.pathID switch
        {
            0 => Vector3.back,
            1 => Vector3.forward,
            _ => Vector3.zero
        })) * rollingSpeed * ball.rollingSpeed;
        moveVector.x *= -1;

        ball.ballRigidbody.velocity = moveVector;
    }
    public override void OnBallStay(BallBehavior ball)
    {
        ball.ballRigidbody.velocity = ball.rollingSpeed * rollingSpeed * ball.ballRigidbody.velocity.normalized;
    }
    public override void OnBallExit(BallBehavior ball)
    {

    }
}
