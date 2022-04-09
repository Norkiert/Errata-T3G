using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedTrack : BasicTrack
{
    public new const float length = (ModelTrack.length + ModelTrack.width) / 2;
    public new const float height = ModelTrack.height;
    public new const float width = length;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/CurvedTrack.prefab";

    [SerializeField] public Transform rotationPoint;

    public new void Awake()
    {
        base.Awake();
    }
    public override void RotateRight()
    {
        transform.Rotate(Vector3.up * 90);
    }
    public override void RotateLeft()
    {
        transform.Rotate(Vector3.up * -90);
    }
    public override void MoveBall(BallBehavior ball)
    {
        InitBallPath(ball);
        var deltaX = (rotationPoint.position.x - ball.transform.position.x);
        var deltaZ = (rotationPoint.position.z - ball.transform.position.z);
        var angle = Quaternion.Euler(0, Mathf.Atan2(deltaZ, deltaX) * Mathf.Rad2Deg, 0) * transform.rotation;
        var moveVector = angle * (Quaternion.Inverse(transform.rotation) * (ball.pathID switch
        {
            0 => Vector3.forward,
            1 => Vector3.back,
            _ => Vector3.zero
        })) * rollingSpeed * ball.rollingSpeed;
        moveVector.x *= -1;
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
