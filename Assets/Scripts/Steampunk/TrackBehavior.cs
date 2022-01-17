using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBehavior : Clickable
{  
    private new void Awake()
    {
        OnClick += RotateTrack;
    }
    public void RotateTrack()
    {
        transform.Rotate(0, 0, 90);
    }
}
