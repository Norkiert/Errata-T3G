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
        var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * Vector3.forward);
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void AlignTo(TrackConnectionInfo tci)
    {
        if(tci.track.transform.parent != transform.parent)
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
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
