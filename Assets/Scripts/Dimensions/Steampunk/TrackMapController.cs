using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TrackMapController : MonoBehaviour
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
    public enum TrackType
    {
        Straight,
        Curved,
        Merger,
        Splitter,
        Cross,
        StraightUpwards,
        Elevator,
    }
    public Transform zeroPoint;
    protected Dictionary<TrackMapPosition, BasicTrack> trackMap = new Dictionary<TrackMapPosition, BasicTrack>();
    public Dictionary<TrackMapPosition, BasicTrack> TrackMap { get { return trackMap; } private set { } }
    public int Count => trackMap.Count;
    public bool Add(BasicTrack track, TrackMapPosition position)
    {
        if (Contains(position))
        {
            Debug.LogError($"Tried to add track at occupied position: {position}.");
            return false;
        }
        else if (Contains(track))
        {
            Debug.LogError("Tried to add track that is already in this TrackGroup.");
            return false;
        }
        else if (track.trackMapController)
        {
            Debug.LogError("Tried to add track that is already in some TrackGroup.");
            return false;
        }
        else
        {
            trackMap[position] = track;
            track.trackMapController = this;
            track.position = position;
            track.InitPos(position);
            return true;
        }
    }
    public void Remove(BasicTrack track)
    {
        if (Contains(track))
            Remove(track.position);
    }
    public void Remove(TrackMapPosition position) => trackMap.Remove(position);
    public bool Occupied(TrackMapPosition position)
    {
        return trackMap.ContainsKey(position) && trackMap[position] != null;
    }
    public bool Contains(TrackMapPosition position) => Occupied(position);
    public bool Contains(BasicTrack track)
    {
        return Contains(track.position) && track.trackMapController == this;
    }
    public BasicTrack Get(TrackMapPosition position)
    {
        if (Occupied(position))
            return trackMap[position];
        else
            return null;
    }
    public BasicTrack[,] GetNeighbors(TrackMapPosition position)
    {
        BasicTrack[,] neighbors = new BasicTrack[(int)BasicTrack.NeighborLevel.end, (int)BasicTrack.NeighborPosition.end];

        for(BasicTrack.NeighborLevel neighborLevel = 0; neighborLevel != BasicTrack.NeighborLevel.end; ++neighborLevel)
        {
            for(BasicTrack.NeighborPosition neighborPosition = 0; neighborPosition != BasicTrack.NeighborPosition.end; ++neighborPosition)
            {
                neighbors[(int)neighborLevel, (int)neighborPosition] = Get(position + neighborLevel.ToPosition() + neighborPosition.ToPosition());
            }
        }

        return neighbors;
    }
    public BasicTrack[,] GetNeighbors(BasicTrack track)
    {
        if (Contains(track))
            return GetNeighbors(track.position);
        else
            return null;
    }

#if UNITY_EDITOR
    [SerializeField] [HideInInspector] List<int> trackMapX = new List<int>();
    [SerializeField] [HideInInspector] List<int> trackMapY = new List<int>();
    [SerializeField] [HideInInspector] List<int> trackMapZ = new List<int>();
    [SerializeField] [HideInInspector] List<BasicTrack> trackMapTracks = new List<BasicTrack>();
    public void OnBeforeSerialize()
    {
        trackMapX.Clear();
        trackMapY.Clear();
        trackMapZ.Clear();
        trackMapTracks.Clear();
        foreach (var element in trackMap)
        {
            trackMapX.Add(element.Key.x);
            trackMapY.Add(element.Key.y);
            trackMapZ.Add(element.Key.z);
            trackMapTracks.Add(element.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        for(int i = 0; i != trackMapTracks.Count; ++i)
        {
            if (trackMapTracks[i])
                trackMap.Add((trackMapX[i], trackMapY[i], trackMapZ[i]), trackMapTracks[i]);
            else
                Debug.LogWarning("Track was deleted out of TrackEditor, null reference exceptions may happen");
        }
        trackMapX.Clear();
        trackMapY.Clear();
        trackMapZ.Clear();
        trackMapTracks.Clear();
    }
#endif
}