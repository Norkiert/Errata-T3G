using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class IsMirror : Interactable
{
    void Update()
    {
        if(IsSelected&&(Input.GetKey(KeyCode.Q)|| Input.GetKey(KeyCode.E)))
        {
            if(Input.GetKey(KeyCode.E))
            {
                Debug.Log("E");
                transform.Rotate(0f,0.2f,0f);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log("Q");
                transform.Rotate(0f,-0.2f, 0f);
            }
        }
    }
}
