using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UnderTrackBox : Clickable
{
    public const string PrefabPath = "Assets/Art/Dimensions/Electrical/Prefabs/wooden_box.prefab";
    public const float height = 0.02152f;

    [SerializeField] public BasicTrack connectedTrack;
    protected Transform player;

    [SerializeField] LayerMask layerMask;

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
            return;
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

        MyTransform.position = connectedTrack.MyTransform.position;

        MyTransform.position -= Vector3.up * (trackHeight * connectedTrack.MyTransform.lossyScale.y + height * MyTransform.localScale.y);
    }
    
    [Button("Update Track Position")]
    public void UpdateTrackPosition()
    {
        var tmc = connectedTrack.trackMapController;

        TrackMapPosition newPosition = ((int)MyTransform.localPosition.x, 0, (int)MyTransform.localPosition.z);

        // if (tmc.Contains(newPosition))
        // {
        //     Debug.LogError($"Cannot move track to ({newPosition.x}; {newPosition.z}) as this place is already occupied.");
        //     return;
        // }

        tmc.Remove(connectedTrack);
        tmc.Add(connectedTrack, newPosition);
    }

    protected void Push()
    {
        if (!player)
        {
            Debug.LogError("Box has no player assigned.");
        }

        Vector3 playerFacing;

        var playerRelativeAngle = (player.rotation * Quaternion.Inverse(MyTransform.parent.rotation)).eulerAngles.y;

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

        if (!connectedTrack.trackMapController.Contains(newPosition) && !Physics.Raycast(MyTransform.position, MyTransform.parent.rotation * playerFacing, out RaycastHit hit, height * 1.5f * playerFacing.z switch
        {
            0 => MyTransform.lossyScale.x,
            _ => MyTransform.lossyScale.z
        }, layerMask))
        {
            var tmc = connectedTrack.trackMapController;
            tmc.Move(connectedTrack, newPosition);
            UpdateBoxPosition();
        }
        else
        {
            newPosition = connectedTrack.position - ((int)playerFacing.x, (int)playerFacing.y, (int)playerFacing.z);
            if (!connectedTrack.trackMapController.Contains(newPosition) && !Physics.Raycast(MyTransform.position, MyTransform.parent.rotation * playerFacing * -1, out RaycastHit hit2, height * 1.5f * playerFacing.z switch
            {
                0 => MyTransform.lossyScale.x,
                _ => MyTransform.lossyScale.z
            }, layerMask))
            {
                var tmc = connectedTrack.trackMapController;
                tmc.Move(connectedTrack, newPosition);
                UpdateBoxPosition();
            }
        }
    }
}
