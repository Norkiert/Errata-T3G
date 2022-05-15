using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ1 : MonoBehaviour
{
    [SerializeField] protected GearedDoor door;
    protected Transform doorTransform;

    [SerializeField] public bool completed = false;

    protected AutoSpawnerTrack spawner;

    protected void Awake()
    {
        doorTransform = door.transform;
        spawner = GetComponent<AutoSpawnerTrack>();
    }
    protected void Update()
    {
        if(!completed && doorTransform.position.y == door.max.y)
        {
            completed = true;
            OnCompletion();
        }
    }
    protected void OnCompletion()
    {
        spawner.SpawnBall();
    }
}
