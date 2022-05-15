using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ1 : MonoBehaviour
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected GearedDoor door;
    protected Transform doorTransform;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();
        doorTransform = door.transform;
    }
    protected void Update()
    {
        if(!qGeneral.completed && doorTransform.position.y == door.max.y)
        {
            OnCompletion();
        }
    }
    protected void OnCompletion()
    {
        qGeneral.OnCompletion();
    }
}
