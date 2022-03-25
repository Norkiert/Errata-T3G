using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCurvedTrack : CurvedTrack
{
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/LeftCurvedTrack.prefab";

    public override void MoveBall(BallBehavior ball)
    {
        var deltaX = (rotationPoint.position.x - ball.transform.position.x);
        var deltaZ = (rotationPoint.position.z - ball.transform.position.z);
        var angle = Quaternion.Euler(0, Mathf.Atan2(deltaZ, deltaX) * Mathf.Rad2Deg, 0) * transform.rotation;
        var moveVector = angle * (Quaternion.Inverse(transform.rotation) * Vector3.back) * rollingSpeed * ball.rollingSpeed;
        moveVector.x *= -1;
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
