using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public static class NeighborLevelExtensions
{
    public static TrackMapPosition ToPosition(this BasicTrack.NeighborLevel level)
    {
        switch (level)
        {
            case BasicTrack.NeighborLevel.same: return (0, 0, 0);
            default:                            return (0, 0, 0);
        }
    }
}
public static class NeighborPositionExtenstions
{
    public static TrackMapPosition ToPosition(this BasicTrack.NeighborPosition position)
    {
        switch (position)
        {
            case BasicTrack.NeighborPosition.Xplus:     return (1, 0, 0);
            case BasicTrack.NeighborPosition.Xminus:    return (-1, 0, 0);
            case BasicTrack.NeighborPosition.Zplus:     return (0, 0, 1);
            case BasicTrack.NeighborPosition.Zminus:    return (0, 0, -1);
            default:                                    return (0, 0, 0);
        }
    }
}

public abstract class BasicTrack : Clickable
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
    [SerializeField] [ReadOnly] public TrackMapController trackMapController;
    public TrackMapPosition position;

    public const float length = 0;
    public const float height = 0;
    public const float width = 0;

    public List<Vector3> rollingPath;   
    [SerializeField] public float rollingSpeed = 0.05f;

    [SerializeField] [ReadOnly] protected NeighborPosition rotation = NeighborPosition.Xplus;
    [SerializeField] [ReadOnly] protected bool rotateable = true;
    public bool Rotateable 
    { 
        get 
        { 
            return rotateable; 
        } 
        set 
        { 
            if(rotateable != value)
            {
                rotateable = value;
                if (value)
                {
                    OnClick += Rotate;  
                }
                else
                {
                    OnClick -= Rotate;
                }
            }
        } 
    }

    public enum NeighborLevel
    {
        same,
        end
    }
    public enum NeighborPosition
    {
        Xplus,
        Zminus,
        Xminus,
        Zplus,
        end
    }

    public BasicTrack[,] NeighborTracks => trackMapController.GetNeighbors(position);
    public TrackConnectionInfo connectedTrack1;
    public TrackConnectionInfo connectedTrack2;
    public BoundingBox boundingBox;
    protected new void Awake()
    {
        if(Rotateable)
            OnClick += Rotate;
        SetRollingPath();
        base.Awake();
    }
    public void Rotate()
    {
        transform.Rotate(Vector3.up * 90);
        ++rotation;
        if (rotation == NeighborPosition.end)
            rotation = 0;
        UpdateConnections();
    }
    // Aligns this track to track given in TrackConnectionInfo(relatively to given's)
    public abstract void AlignTo(TrackConnectionInfo tci);
    public abstract void UpdateConnections();
    public abstract void SetRollingPath();
    public class TrackConnectionInfo
    {
        public static implicit operator BasicTrack(TrackConnectionInfo tci) => tci.track;

        public BasicTrack track;
        public NeighborLevel level;
        public NeighborPosition position;
        public TrackConnectionInfo(BasicTrack basicTrack, NeighborLevel neighborLevel, NeighborPosition neighborPosition)
        {
            track = basicTrack;
            level = neighborLevel;
            position = neighborPosition;
        }
        public TrackConnectionInfo(NeighborLevel neighborLevel, NeighborPosition neighborPosition)
        {
            track = null;
            level = neighborLevel;
            position = neighborPosition;
        }
        public TrackConnectionInfo()
        {
            track = null;
            level = 0;
            position = 0;
        }
        public TrackConnectionInfo Mirror()
        {
            TrackConnectionInfo toReturn = new TrackConnectionInfo();
            toReturn.position = (NeighborPosition)((int)(position + (int)NeighborPosition.end / 2) % (int)NeighborPosition.end);
            toReturn.level = (NeighborLevel)((int)(level + (int)NeighborLevel.end / 2) % (int)NeighborLevel.end);
            return toReturn;
        }
        public TrackMapPosition ToPosition()
        {
            return level.ToPosition() + position.ToPosition();
        }
    }

#if UNITY_EDITOR // backup
    [SerializeField] public int _posX;
    [SerializeField] public int _posY;
    [SerializeField] public int _posZ;
    public void OnBeforeSerialize()
    {
        (_posX, _posY, _posZ) = position;   
    }
    public void OnAfterDeserialize()
    {
        position = (_posX, _posY, _posZ);
        UpdateConnections();
    }
#endif
}
