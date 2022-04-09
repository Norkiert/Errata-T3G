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
    public const string prefabPath = "";

    [SerializeField] public float rollingSpeed = 1f;
    [SerializeField] public List<Transform> pathBeginPoints;

    [SerializeField] public bool rotateable = true;
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
                    OnClick += RotateRight;  
                }
                else
                {
                    OnClick -= RotateRight;
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
    protected new void Awake()
    {
        if(Rotateable)
            OnClick += RotateRight;
        base.Awake();
    }
    public abstract void RotateRight();
    public abstract void RotateLeft();
    public void InitBallPath(BallBehavior ball)
    {
        if (ball.pathID == -1)
        {
            float closestDistance = float.MaxValue;
            for(int i = 0; i != pathBeginPoints.Count; ++i)
            {
                float distanceOfCurrent = Vector3.Distance(ball.transform.position, pathBeginPoints[i].position);
                if(distanceOfCurrent < closestDistance)
                {
                    closestDistance = distanceOfCurrent;
                    ball.pathID = i;
                }
            }
        }
    }
    public abstract void MoveBall(BallBehavior ball);
    public abstract void InitPos(TrackMapPosition tmp);
    [field: SerializeField] [HideInInspector] public TrackMapController.TrackType TrackType { get; set; }
    // transform.position is always not correct
    public Vector3 GetPosition()
    {
        Vector3 toReturn = trackMapController.zeroPoint.position;
        toReturn += new Vector3(position.x, position.y, position.z) * ModelTrack.trackMapCellSize;
        return toReturn;
    }
    public Vector3 GetLocalPosition()
    {
        Vector3 toReturn = trackMapController.zeroPoint.localPosition;
        toReturn += new Vector3(position.x, position.y, position.z) * ModelTrack.trackMapCellSize;
        return toReturn;
    }
    public class TrackConnectionInfo
    {
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
    [SerializeField] [HideInInspector] private int _posX;
    [SerializeField] [HideInInspector] private int _posY;
    [SerializeField] [HideInInspector] private int _posZ;
    public void OnBeforeSerialize()
    {
        (_posX, _posY, _posZ) = position;
    }
    public void OnAfterDeserialize()
    {
        position = (_posX, _posY, _posZ);
    }
#endif
}