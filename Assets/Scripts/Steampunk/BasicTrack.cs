using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;


#if UNITY_EDITOR
[RequireComponent(typeof(BoundingBox))]
#endif
public abstract class BasicTrack : Clickable
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
    public const float length = 0;
    public const float height = 0;
    public const float width = 0;

    [SerializeField] [ReadOnly] public List<Vector3> rollingPath;
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

    public BasicTrack[,] neighborTracks = new BasicTrack[(int)NeighborLevel.end, (int)NeighborPosition.end];
#if UNITY_EDITOR
    [SerializeField] [HideInInspector] public BasicTrack[] _neighborTracks = new BasicTrack[(int)NeighborLevel.end * (int)NeighborPosition.end];
#endif
    public TrackConnectionInfo connectedTrack1;
    public TrackConnectionInfo connectedTrack2;
    public BoundingBox boundingBox;
#if UNITY_EDITOR
    public void OnBeforeSerialize()
    {
        for(int i = 0; i < _neighborTracks.Length; ++i)
        {
            _neighborTracks[i] = neighborTracks[i / (int)NeighborPosition.end, i % (int)NeighborPosition.end];
        }
    }
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < _neighborTracks.Length; ++i)
        {
            neighborTracks[i / (int)NeighborPosition.end, i % (int)NeighborPosition.end] = _neighborTracks[i];
        }
        UpdateConnections();
    }
#endif
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
        public static TrackConnectionInfo Mirror(TrackConnectionInfo tci)
        {
            TrackConnectionInfo toReturn = new TrackConnectionInfo();
            toReturn.position = (NeighborPosition)((int)(tci.position + (int)NeighborPosition.end / 2) % (int)NeighborPosition.end);
            toReturn.level = (NeighborLevel)((int)(tci.level + (int)NeighborLevel.end / 2) % (int)NeighborLevel.end);
            return toReturn;
        }
    }
}
