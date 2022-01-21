using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;

public class TrackBehavior : Clickable
{
    [SerializeField] public List<TrackBehavior> connections;
    [SerializeField] public float rotationAngle = 90;

    private new void Awake()
    {
        OnClick += RotateTrack;
    }
    public void RotateTrack()
    {
        transform.Rotate(0, 0, rotationAngle);
    }
    #region -Tracks Editor-

    private List<TrackBehavior> connectionsBackup;

    public void UpdateConnections()
    {
        if (connectionsBackup.Count > connections.Count) // removed connection from list
        {
            foreach (var fromBackup in connectionsBackup)
            {
                if (fromBackup && !connections.Contains(fromBackup))
                {
                    fromBackup.connections.Remove(this);
                }
            }
            connectionsBackup = new List<TrackBehavior>(connections);
        }
        else if (connectionsBackup.Count < connections.Count) // added connection to list
        {
            foreach (var fromCurrent in connections)
            {
                if (fromCurrent && !connectionsBackup.Contains(fromCurrent) && !fromCurrent.connections.Contains(this))
                {
                    fromCurrent.connections.Add(this);
                }
            }
            connectionsBackup = new List<TrackBehavior>(connections);
        }
    }

    #endregion
}
