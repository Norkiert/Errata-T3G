using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkQ1 : MonoBehaviour
{
    protected SteampunkQGeneral qGeneral;

    [SerializeField] protected GearedDoor door;

    protected void Awake()
    {
        qGeneral = GetComponent<SteampunkQGeneral>();
    }
    protected void Update()
    {
        if(!qGeneral.completed && door.MyTransform.position.y == door.max.y)
        {
            OnCompletion();
        }
    }
    protected void OnCompletion()
    {
        qGeneral.OnCompletion();
    }
}
