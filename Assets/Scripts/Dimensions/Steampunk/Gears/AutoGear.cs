using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGear : Gear
{
    [SerializeField] public float anglePerSecond;

    void Update()
    {
        Power();
    }
    void Power()
    {
        float angle = anglePerSecond * Time.deltaTime;
        Rotate(angle);
    }
}
