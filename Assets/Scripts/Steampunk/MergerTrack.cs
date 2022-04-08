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
        transform.localPosition = GetLocalPosition();
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
        InitBallPath(ball);
        var rotationPoint = ball.pathID switch
        {
            0 => rotationPoint_0,
            1 => rotationPoint_1,
            _ => throw new System.Exception("Invalid BallBehavior's pathID.")
        };
        var deltaX = (rotationPoint.position.x - ball.transform.position.x);
        var deltaZ = (rotationPoint.position.z - ball.transform.position.z);
        var angle = Quaternion.Euler(0, Mathf.Atan2(deltaZ, deltaX) * Mathf.Rad2Deg, 0) * transform.rotation;
        var moveVector = angle * (Quaternion.Inverse(transform.rotation) * (ball.pathID switch
        {
            0 => Vector3.back,
            1 => Vector3.forward,
            _ => Vector3.zero
        })) * rollingSpeed * ball.rollingSpeed;
        moveVector.x *= -1;
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void AlignTo(TrackConnectionInfo tci)
    {
        if (tci.track.transform.parent != transform.parent)
        {
            Debug.LogError("Cannot align to track with different parent!");
        }
        else if (tci.track.NeighborTracks[(int)tci.level, (int)tci.position] != this && tci.track.NeighborTracks[(int)tci.level, (int)tci.position])
        {
            Debug.LogError($"Cannot align to track at that position: {{{(int)tci.level}}}, {{{(int)tci.position}}}");
        }
        else
        {
            transform.position = new Vector3(tci.track.transform.position.x, tci.track.transform.position.y, tci.track.transform.position.z);
            transform.localPosition = new Vector3(tci.track.transform.localPosition.x, tci.track.transform.localPosition.y, tci.track.transform.localPosition.z);
            switch (tci.position)
            {
                case NeighborPosition.Xplus:
                    transform.localPosition += length * Vector3.right;
                    break;
                case NeighborPosition.Zminus:
                    transform.localPosition += length * Vector3.back;
                    break;
                case NeighborPosition.Xminus:
                    transform.localPosition += length * Vector3.left;
                    break;
                case NeighborPosition.Zplus:
                    transform.localPosition += length * Vector3.forward;
                    break;
            }
            switch (tci.level)
            {
                case NeighborLevel.same:
                    break;
            }
        }
    }
}
