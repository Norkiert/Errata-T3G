using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Q3CollectorMovingElement : MonoBehaviour
{
    [SerializeField][ReadOnly] protected int isMoving = 0;
    [SerializeField] protected float movingSpeed = 0.1f;

    [SerializeField] protected LayerMask ballLayer;

    [SerializeField][ReadOnly] public List<GameObject> balls;
    [SerializeField][ReadOnly] public List<Rigidbody> ballsRigidbodies;

    [SerializeField][ReadOnly] protected float movedDistance = 0f;
    [SerializeField] protected float distance = 0f;

    [SerializeField] protected float timeToWait = 0f;
    [SerializeField][ReadOnly] protected float timeElapsed = 0f;
    [SerializeField][ReadOnly] protected bool isWaiting = false;

    protected void OnCollisionEnter(Collision collision)
    {
        if (isMoving == 0)
        {
            if ((1 << collision.gameObject.layer & ballLayer.value) != 0)
            {
                if (!balls.Contains(collision.gameObject))
                {
                    balls.Add(collision.gameObject);
                    ballsRigidbodies.Add(collision.gameObject.GetComponent<Rigidbody>());

                    var ballPos = transform.position;
                    ballPos.y = collision.gameObject.transform.position.y;
                    ballPos += (balls.Count - 3) * StraightTrack.length * transform.lossyScale.z * Vector3.forward;

                    balls[balls.Count - 1].transform.position = ballPos;
                    ballsRigidbodies[ballsRigidbodies.Count - 1].velocity = Vector3.zero;

                    if (balls.Count == 5)
                    {
                        BeginDropping();
                        balls.Clear();
                    }
                }
            }
        }
    }

    protected void Update()
    {
        if(balls.Count != 0)
        {
            foreach(Rigidbody rigidbody in ballsRigidbodies)
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
        if (isWaiting)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= timeToWait)
            {
                timeElapsed = 0;
                isWaiting = false;
                BeginClosing();
            }
        }
        switch (isMoving)
        {
            case 1:
                if(movedDistance >= distance)
                {
                    StopAllCoroutines();
                    isMoving = 0;
                    isWaiting = true;
                    balls.Clear();
                    ballsRigidbodies.Clear();
                }
                break;
            case -1:
                if(movedDistance <= 0)
                {
                    StopAllCoroutines();
                    isMoving = 0;
                    isWaiting = false;
                }
                break;
        }
    }

    protected void BeginDropping()
    {
        if (isMoving != 0) return;

        isMoving = 1;
        StartCoroutine(Open());
    }
    protected void BeginClosing()
    {
        if (isMoving != 0) return;

        isMoving = -1;
        StartCoroutine(Close());
    }
    protected IEnumerator Open()
    {
        for(; ; )
        {
            transform.localPosition += transform.forward * Time.deltaTime * movingSpeed;
            movedDistance += Time.deltaTime * movingSpeed;
            yield return null;
        }
    }
    protected IEnumerator Close()
    {
        for(; ; )
        {
            transform.localPosition -= transform.forward * Time.deltaTime * movingSpeed;
            movedDistance -= Time.deltaTime * movingSpeed;
            yield return null;
        }
    }
}
