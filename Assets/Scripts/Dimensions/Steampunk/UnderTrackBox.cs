using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UnderTrackBox : Clickable
{
    public const string PrefabPath = "Assets/Art/Dimensions/Electrical/Prefabs/wooden_box.prefab";
    public const float height = 0.8384452f;
    public const float defaultScale = 83.97792f;

    [SerializeField] protected BasicTrack connectedTrack;
    protected Transform player;

    protected override void Awake()
    {
        OnClick += Push;
    }

    protected void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    [Button("Update Box Position")]
    protected void UpdateBoxPosition()
    {
        if (!connectedTrack)
        {
            Debug.LogError("Box has no track assigned.");
        }

        float trackHeight = connectedTrack.TrackType switch
        {
            TrackMapController.TrackType.Straight => StraightTrack.height,
            TrackMapController.TrackType.Curved => CurvedTrack.height,
            TrackMapController.TrackType.Merger => MergerTrack.height,
            TrackMapController.TrackType.Splitter => SplitterTrack.height,
            TrackMapController.TrackType.Cross => CrossTrack.height,
            TrackMapController.TrackType.StraightUpwards => StraightUpwardsTrack.height,
            TrackMapController.TrackType.Elevator => ElevatorTrack.height,
            _ => BasicTrack.height
        };
        transform.localPosition = connectedTrack.transform.localPosition - Vector3.up * (trackHeight + height * transform.localScale.y / defaultScale);
    }
    
    protected void Push()
    {
        if (!player)
        {
            Debug.LogError("Box has no player assigned.");
        }

        Vector3 playerFacing;

        var playerRelativeAngle = (player.rotation * Quaternion.Inverse(transform.parent.rotation)).eulerAngles.y;

        if (playerRelativeAngle > 45f && playerRelativeAngle <= 135f)           // Xplus
        {
            playerFacing = Vector3.right;
        }
        else if (playerRelativeAngle > 135f && playerRelativeAngle <= 225f)     // Zminus
        {
            playerFacing = Vector3.back;
        }
        else if (playerRelativeAngle > 225f && playerRelativeAngle <= 315f)     // Xminus
        {
            playerFacing = Vector3.left;
        }
        else // if (playerRelativeAngle > 315f || playerRelativeAngle <= 45f)   // Zplus
        {
            playerFacing = Vector3.forward;
        }

        var newPosition = connectedTrack.position + ((int)playerFacing.x, (int)playerFacing.y, (int)playerFacing.z);

        if (!connectedTrack.trackMapController.Contains(newPosition))
        {
            var tmc = connectedTrack.trackMapController;
            tmc.Remove(connectedTrack);
            tmc.Add(connectedTrack, newPosition);
            UpdateBoxPosition();
        }
    }
}
