using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class StraightTrack : BasicTrack
{
    public new const float length = ModelTrack.length;
    public new const float height = ModelTrack.height;
    public new const float width = ModelTrack.width;

    protected static TrackConnectionInfo defaultConnection1 = new TrackConnectionInfo(NeighborLevel.same, NeighborPosition.Xplus);
    protected static TrackConnectionInfo defaultConnection2 = new TrackConnectionInfo(NeighborLevel.same, NeighborPosition.Xminus);

    protected new void Awake()
    {
        base.Awake();
    }
    public override void UpdateConnections()
    {
        if (trackMapController == null)   
            return;
        connectedTrack1 = new TrackConnectionInfo();
        connectedTrack2 = new TrackConnectionInfo();
        connectedTrack1.position = (NeighborPosition)(((int)defaultConnection1.position + (int)rotation) % (int)NeighborPosition.end);
        connectedTrack2.position = (NeighborPosition)(((int)defaultConnection2.position + (int)rotation) % (int)NeighborPosition.end);
        connectedTrack1.level = defaultConnection1.level;
        connectedTrack2.level = defaultConnection2.level;
        connectedTrack1.track = NeighborTracks[(int)connectedTrack1.level, (int)connectedTrack1.position];  
        connectedTrack2.track = NeighborTracks[(int)connectedTrack2.level, (int)connectedTrack2.position];
    }
    public override void SetRollingPath()
    {
        rollingPath = new List<Vector3>();

        var firstPoint = defaultConnection1.ToPosition();
        var secondPoint = defaultConnection2.ToPosition();

        rollingPath.Add(new Vector3(length / 2f * firstPoint.x, 0, length / 2f * firstPoint.z));
        rollingPath.Add(new Vector3(length / 2f * secondPoint.x, 0, length / 2f * secondPoint.z));
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

            UpdateConnections();
            tci.track.UpdateConnections();
        }
    }
}
