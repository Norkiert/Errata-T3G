using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Q2Capsule : MonoBehaviour
{
    public void SLideCapsuleDown()
    {
        transform.DOMoveY(2f, 5f);
    }
}
