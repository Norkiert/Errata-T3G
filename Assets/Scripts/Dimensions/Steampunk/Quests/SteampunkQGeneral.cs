using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQGeneral : MonoBehaviour
{
    [SerializeField] public bool completed = false;

    protected AutoSpawnerTrack spawner;

    protected void Awake()
    {
        spawner = GetComponent<AutoSpawnerTrack>();
    }
    public void OnCompletion()
    {
        completed = true;
        spawner.SpawnBall();
    }
}
