using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class StraightTrack : BasicTrack
{
    public new const float length = ModelTrack.length;
    public new const float height = ModelTrack.height;
    public new const float width = ModelTrack.width;


    protected new void Awake()
    {
        base.Awake();
    }
    public override void MoveBall(BallBehavior ball)
    {
        ball.ballRigidbody.velocity = rollingSpeed * ball.rollingSpeed * rotation.ToPosition().ToVector3();
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
                    transform.localPosition += ModelTrack.length * Vector3.right;
                    break;
                case NeighborPosition.Zminus:
                    transform.localPosition += ModelTrack.length * Vector3.back;
                    break;
                case NeighborPosition.Xminus:
                    transform.localPosition += ModelTrack.length * Vector3.left;
                    break;
                case NeighborPosition.Zplus:
                    transform.localPosition += ModelTrack.length * Vector3.forward;
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
