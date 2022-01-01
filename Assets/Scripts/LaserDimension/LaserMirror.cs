using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserMirror : Interactable
{
    [SerializeField] private KeyCode roatateRightKey = KeyCode.E;
    [SerializeField] private KeyCode roatateLeftKey = KeyCode.Q;

    private void Update()
    {
        if (IsSelected && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)))
        {
            if (Input.GetKey(roatateRightKey))
            {
                transform.Rotate(0f, 0.2f, 0f);
            }
            if (Input.GetKey(roatateLeftKey))
            {
                transform.Rotate(0f, -0.2f, 0f);
            }
        }
    }
}
