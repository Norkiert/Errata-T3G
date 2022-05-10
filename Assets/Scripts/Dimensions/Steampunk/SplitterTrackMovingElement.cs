using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTrackMovingElement : MonoBehaviour
{
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private SplitterTrack splitterTrack;
    public bool Rotating { get; private set; }
    private float totalRotation = 0f;

    private BallBehavior ball;
    
    private void Update()
    {
        if (Rotating)
        {
            if (splitterTrack.hammerFacingRight && totalRotation >= 100f)
            {
                StopAllCoroutines();
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 310f, transform.localEulerAngles.z);
                Rotating = false;
            }
            else if (!splitterTrack.hammerFacingRight && totalRotation >= 100f)
            {
                StopAllCoroutines();
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 50f, transform.localEulerAngles.z);
                Rotating = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!Rotating)
        {
            if ((1 << collision.gameObject.layer & ballLayer.value) != 0 && (ball && ball.gameObject != collision.gameObject || !ball))
            {
                ball = collision.gameObject.GetComponent<BallBehavior>();
                Rotating = true;
                splitterTrack.hammerFacingRight = !splitterTrack.hammerFacingRight;
                totalRotation = 0f;
                StopAllCoroutines();
                StartCoroutine(splitterTrack.hammerFacingRight ? RotateRight() : RotateLeft());
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!Rotating)
        {
            if ((1 << collision.gameObject.layer & ballLayer.value) != 0)
            {
                StartCoroutine(BallTimer());
            }
        }
    }
    private IEnumerator BallTimer()
    {
        const float timeToWait = 1;
        float timeElapsed = 0;
        for(; ; )
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= timeToWait)
            {
                ball = null;
                yield break;
            }
            yield return null;
        }
    }
    public void RotateLeftInstant() => transform.Rotate(Vector3.up, 100f);
    public void RotateRightInstant() => transform.Rotate(Vector3.up, -100f);
    public IEnumerator RotateLeft()
    {
        for (; ; )
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            totalRotation += rotationSpeed * Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator RotateRight()
    {
        for(; ; )
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            totalRotation += rotationSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
