using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizedMonoBehaviour : MonoBehaviour
{
    private Transform myTransform;
    public Transform MyTransform
    {
        get
        {
            if (myTransform == null)
            {
                myTransform = transform;
            }
            return myTransform;
        }
    }

    private GameObject myGameObject;
    public GameObject MyGameObject
    {
        get
        {
            if (myGameObject == null)
            {
                myGameObject = gameObject;
            }
            return myGameObject;
        }
    }
}
