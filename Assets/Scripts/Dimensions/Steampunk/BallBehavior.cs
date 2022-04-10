using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
public class BallBehavior : ObjectGroundChecker
{
    [SerializeField] protected float timeLimit = 5f;
    [SerializeField] [ReadOnly] protected float timeElapsed = 0f;
    [SerializeField] protected bool groudTimerRunning = true;

    [SerializeField] protected bool isDestroying = false;
    [SerializeField] protected float destructionSpeed = 0.005f;

    public SphereCollider ballCollider;
    public Rigidbody ballRigidbody;
    public float realRadius;

    [SerializeField] protected LayerMask trackLayer;
    [SerializeField] [ReadOnly] protected BasicTrack currentTrack;
    [SerializeField] [ReadOnly] protected bool onTrack;
    [SerializeField] public float rollingSpeed = 1f;
    [SerializeField] [ReadOnly] public float velocity;
    [SerializeField] [ReadOnly] public int pathID = -1;

    protected void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
        realRadius = ballCollider.bounds.size.y / 2f;
    }

    protected void Update()
    {
        #region -Ground Destruction-

        if (isDestroying)
        {
            Destroy();
        }
        else if (isGrounded)
        {
            if (groudTimerRunning)
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

        #endregion
    }

    protected void OnCollisionStay(Collision collision)
    {
        if ((1 << collision.gameObject.layer & trackLayer.value) != 0)
        {
            if (!currentTrack)
            {
                currentTrack = collision.gameObject.transform.parent.gameObject.GetComponent<BasicTrack>();
                currentTrack.InitBallMovement(this);
            }
            currentTrack.MoveBall(this);
            velocity = ballRigidbody.velocity.magnitude;
        }
    }

    protected new void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer & trackLayer.value) != 0)
        {
            onTrack = false;
            currentTrack = null;
            ballRigidbody.WakeUp();
            pathID = -1;
        }
        else
        {
            base.OnCollisionExit(collision);
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
        ballRigidbody.useGravity = false;
    }
}
