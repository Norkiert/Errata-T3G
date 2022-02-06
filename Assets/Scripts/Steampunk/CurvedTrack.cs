using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedTrack : BasicTrack
{
    public new const float length = 0.1895f * 2;
    public new const float height = 0.05333013f * 2;
    public new const float width = length;

    public new void Awake()
    {
        base.Awake();
    }
    public override void UpdateConnections()
    {
        
    }
    public override void SetRollingPath()
    {
        
    }
    public override void AlignTo(TrackConnectionInfo tci)
    {
        if (tci.track.transform.parent != transform.parent)
        {
            Debug.LogError("Cannot align to track with different parent!");
        }
        else
        {

        }
    }
}
