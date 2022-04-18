using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public struct SVector3
 {
     public float x;
     public float y;
     public float z;
     
     public SVector3(float rX, float rY, float rZ)
     {
         x = rX;
         y = rY;
         z = rZ;
     }
     
     public override string ToString()
     {
         return string.Format("[{0}, {1}, {2}]", x, y, z);
     }
     
     public static implicit operator Vector3(SVector3 rValue)
     {
         return new Vector3(rValue.x, rValue.y, rValue.z);
     }
     
     public static implicit operator SVector3(Vector3 rValue)
     {
         return new SVector3(rValue.x, rValue.y, rValue.z);
     }
 }
