using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] protected float xAnglePerSecond = 0;
    [SerializeField] protected float yAnglePerSecond = 0;
    [SerializeField] protected float zAnglePerSecond = 0;
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(xAnglePerSecond, yAnglePerSecond, zAnglePerSecond) * Time.deltaTime;
    }
}
