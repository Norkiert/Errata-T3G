using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BallSpitter : OptimizedMonoBehaviour
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

    [SerializeField] protected float timeToSpawn;
    [SerializeField, ReadOnly] protected float timeElapsed;

    protected void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeToSpawn)
        {
            timeElapsed = 0;
            SpawnBall();
        }
    }
    public void BeginRotateRight()
    {
        StartCoroutine(RotateRight());
    }
    protected IEnumerator RotateRight() => RotateX(true);
    public void BeginRotateLeft()
    {
        StartCoroutine(RotateLeft());
    }
    protected IEnumerator RotateLeft() => RotateX(false);
    protected IEnumerator RotateX(bool right)
    {
        float totalRotation = 0f;
        for (; ; )
        {
            MyTransform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime * (right ? 1 : -1));
            totalRotation += RotationSpeed * Time.deltaTime;
            if (totalRotation >= rotationAngle * 2)
            {
                if (right)
                {
                    RotateRightInstant();
                }
                else
                {
                    RotateLeftInstant();
                }

                yield break;
            }
            yield return null;
        }
    }
    public void RotateRightInstant() => RotateXInstant(true);
    public void RotateLeftInstant() => RotateXInstant(false);
    protected void RotateXInstant(bool right)
    {
        if(right && MyTransform.rotation.z != rotationAngle)
        {
            MyTransform.localEulerAngles = Vector3.forward * rotationAngle;
        }
        else if(!right && MyTransform.rotation.z != 360 - rotationAngle)
        {
            MyTransform.localEulerAngles = Vector3.forward * -rotationAngle;
        }
    }
    protected void SpawnBall()
    {
        BallPool.GetBall(spawnPoint.position);
    }
}
