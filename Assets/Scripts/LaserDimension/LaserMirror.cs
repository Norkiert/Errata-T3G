using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserMirror : Interactable
{
    [SerializeField] private KeyCode roatateRightKey = KeyCode.E;
    [SerializeField] private KeyCode roatateLeftKey = KeyCode.Q;

    [SerializeField] private float roataionSpeed = 20;

    private void Update()
    {
        if (IsSelected && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)))
        {
            if (Input.GetKey(roatateRightKey))
            {
                transform.Rotate(Vector3.up * roataionSpeed * Time.deltaTime);
            }
            if (Input.GetKey(roatateLeftKey))
            {
                transform.Rotate(-Vector3.up * roataionSpeed * Time.deltaTime);
            }
        }
    }
}
