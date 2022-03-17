using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurvedTrack : BasicTrack
{
    public new const float length = (ModelTrack.length + ModelTrack.width) / 2;
    public new const float height = ModelTrack.height;
    public new const float width = length;
    public new const string prefabPath = "";
    public const float radius = length - ModelTrack.width / 2;
    public const float realLength = 0.5f * Mathf.PI * radius;
    

    public new void Awake()
    {
        base.Awake();
    }
    public override void RotateRight()
    {
        transform.RotateAround(GetPosition(), Vector3.up, 90);
    }
    public override void RotateLeft()
    {
        transform.RotateAround(GetPosition(), Vector3.up, -90);
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
