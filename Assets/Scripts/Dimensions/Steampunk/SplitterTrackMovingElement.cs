using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTrackMovingElement : MonoBehaviour
{
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private SplitterTrack splitterTrack;
    private bool rotating;
    private float totalRotation = 0f;
    
    private void Update()
    {
        //Debug.Log(rotating);
        //Debug.Log(splitterTrack.hammerFacingRight);
        //Debug.Log(totalRotation);
        if (rotating)
        {
            if (splitterTrack.hammerFacingRight && totalRotation >= 100f)
            {
                Debug.Log("bruh L");
                StopAllCoroutines();
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 310f, transform.localEulerAngles.z);
                rotating = false;
            }
            else if (!splitterTrack.hammerFacingRight && totalRotation >= 100f)
            {
                Debug.Log("bruh R");
                StopAllCoroutines();
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 50f, transform.localEulerAngles.z);
                rotating = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!rotating)
        {
            if ((1 << collision.gameObject.layer & ballLayer.value) != 0)
            {
                rotating = true;
                splitterTrack.hammerFacingRight = !splitterTrack.hammerFacingRight;
                totalRotation = 0f;
                StartCoroutine(splitterTrack.hammerFacingRight ? RotateRight() : RotateLeft());
            }
        }
    }
    public void RotateLeftInstant() => transform.Rotate(Vector3.up, 100f);
    public void RotateRightInstant() => transform.Rotate(Vector3.up, -100f);
    public IEnumerator RotateLeft()
    {
        for (; ; )
        {
            //Debug.Log("RotateLeft");
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            totalRotation += rotationSpeed * Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator RotateRight()
    {
        for(; ; )
        {
            //Debug.Log("RotateRight");
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
            totalRotation += rotationSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
