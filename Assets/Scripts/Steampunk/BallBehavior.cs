using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
public class BallBehavior : ObjectGroundChecker
{
    [SerializeField] protected float timeLimit = 5f;
    [SerializeField] [field: ReadOnly] protected float timeElapsed = 0f;
    [SerializeField] protected bool timerRunning = true;

    [SerializeField] protected bool isDestroying = false;
    [SerializeField] protected float destructionSpeed = 0.005f;

    protected SphereCollider ballCollider;
    protected float realRadius;
    
    protected void Awake()
    {
        ballCollider = GetComponent<SphereCollider>();
        realRadius = ballCollider.bounds.size.y / 2f;
    }

    protected void Update()
    {
        if (isDestroying)
        {
            Destroy();
        }
        else if (isGrounded)
        {
            if (timerRunning)
            {
                UpdateTime();
            }
            if (timeElapsed >= timeLimit)
            {
                isDestroying = true;
                InitDestruction();
            }
        }
        else
        {
            timeElapsed = 0f;
        }
    }

    protected void UpdateTime()
    {
        timeElapsed += Time.deltaTime;
    }

    protected void Destroy()
    {
        transform.position = transform.position + Vector3.down * destructionSpeed;
        if (transform.position.y + realRadius < groundTransform.position.y)
        {
            Destroy(gameObject);
        }
    }

    protected void InitDestruction()
    {
        ballCollider.enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
    }
}
