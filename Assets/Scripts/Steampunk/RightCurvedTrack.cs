using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCurvedTrack : CurvedTrack
{
    public override void MoveBall(BallBehavior ball)
    {
        float anglePerMove = 90 * (ball.rollingSpeed * rollingSpeed / realLength);
        ball.transform.RotateAround((GetPosition() + (transform.rotation * (Vector3.left + Vector3.back)) * ModelTrack.length / 2), Vector3.up, -anglePerMove);
    }
}
