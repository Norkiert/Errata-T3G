using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpitter : MonoBehaviour
{
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected BallBehavior ballPrefab;

    [SerializeField] protected SplitterTrackMovingElement hammer;
    protected float RotationSpeed
    {
        get
        {
            return hammer.rotationSpeed * rotationAngle / 50f;
        }
    }

    [SerializeField] protected float rotationAngle;

    public void BeginRotateRight()
    {
        StartCoroutine(RotateRight());
    }
    protected IEnumerator RotateRight()
    {
        float totalRotation = 0f;
        for (; ; )
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
            totalRotation += RotationSpeed * Time.deltaTime;
            if (totalRotation >= rotationAngle * 2)
            {
                RotateRightInstant();

                SpawnBall();

                yield break;
            }
            yield return null;
        }
    }
    public void BeginRotateLeft()
    {
        StartCoroutine(RotateLeft());
    }
    protected IEnumerator RotateLeft()
    {
        float totalRotation = 0f;
        for (; ; )
        {
            transform.Rotate(Vector3.forward, -RotationSpeed * Time.deltaTime);
            totalRotation += RotationSpeed * Time.deltaTime;
            if (totalRotation >= rotationAngle * 2)
            {
                RotateLeftInstant();

                SpawnBall();

                yield break;
            }
            yield return null;
        }
    }
    public void RotateRightInstant()
    {
        if (transform.rotation.z != rotationAngle)
        {
            transform.localEulerAngles = Vector3.forward * rotationAngle;
        }
    }
    public void RotateLeftInstant()
    {
        if(transform.rotation.z != 360 - rotationAngle)
        {
            transform.localEulerAngles = Vector3.forward * -rotationAngle;
        }
    }
    protected void SpawnBall()
    {
        var ball = Instantiate(ballPrefab, spawnPoint.position, new Quaternion());
    }
}
