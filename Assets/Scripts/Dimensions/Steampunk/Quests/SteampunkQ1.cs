using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ1 : MonoBehaviour
{
    [SerializeField] protected GearedDoor door;
    protected Transform doorTransform;

    [SerializeField] public bool completed = false;

    protected void Awake()
    {
        doorTransform = door.transform;
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

    }
}
