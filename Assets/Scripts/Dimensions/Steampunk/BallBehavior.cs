using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class BallBehavior : ObjectGroundChecker
{
    [SerializeField] public float timeLimit = 5f;
    [SerializeField] [ReadOnly] public float timeElapsed = 0f;
    [SerializeField] public bool groudTimerRunning = true;

    [SerializeField] public bool isDestroying = false;
    [SerializeField] protected float _destructionSpeed = 0.005f;
    [SerializeField] protected float groundY;

    [SerializeField] public Renderer ballRenderer;
    [SerializeField] public Material defaultMaterial;
    [SerializeField] public Material destructionMaterial;
    [SerializeField] protected float notVisible = -1.44f;
    [SerializeField] protected float visible = 1.78f;
    [SerializeField] protected float destructionSpeed = 1f;

    public SphereCollider ballCollider;
    public Rigidbody ballRigidbody;
    public float realRadius;

    [SerializeField] protected LayerMask trackLayer;

    [SerializeField] [ReadOnly] public BasicTrack currentTrack;
    [SerializeField] [HideInInspector] public BasicTrack lastTrack;
    [SerializeField, HideInInspector] public ImpulseTrack impulseTrack;
    [SerializeField] [ReadOnly] public bool onTrack;

    [SerializeField] public float rollingSpeed = 1f;
    [SerializeField] [ReadOnly] public float velocity;

    [SerializeField] [ReadOnly] public int pathID = -1;
    [SerializeField] [HideInInspector] private int pathIDCopy;

    protected void Awake()
    {
        realRadius = ballCollider.bounds.size.y / 2f;
    }

    protected void Update()
    {
        #region -Ground Destruction-

        if (isGrounded)
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

    protected new void OnCollisionStay(Collision collision)
    {
        var collisionGO = collision.gameObject;
        var collisionT = collision.transform;

        if ((1 << collisionGO.layer & trackLayer.value) != 0)
        {
            if (!currentTrack)
            {
                currentTrack = collisionT.parent.gameObject.GetComponent<BasicTrack>();

                if (currentTrack.TryGetComponent(out impulseTrack) && impulseTrack.impulseMode)
                {
                    impulseTrack.RegisterImpulse(this);
                }

                if (currentTrack != lastTrack && currentTrack)
                {
                    currentTrack.balls.Add(this);
                    currentTrack.OnBallEnter(this);

                    lastTrack = currentTrack;
                    pathIDCopy = pathID;
                }
                else
                {
                    pathID = pathIDCopy;
                }
            }
            if(currentTrack) currentTrack.OnBallStay(this);
            velocity = ballRigidbody.velocity.magnitude;
        }
        else
        {
            base.OnCollisionStay(collision);
        }
    }

    protected new void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer & trackLayer.value) != 0)
        {
            if (currentTrack)
            {
                if(impulseTrack && !impulseTrack.impulseMode)
                {
                    impulseTrack.RegisterImpulse(this);
                }
                currentTrack.balls.Remove(this);
                currentTrack.OnBallExit(this);
            }

            onTrack = false;
            currentTrack = null;
            impulseTrack = null;
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

    protected IEnumerator Destroy()
    {
        /*MyTransform.position = MyTransform.position + Vector3.down * _destructionSpeed;
        if (groundTransform && MyTransform.position.y + realRadius < groundTransform.position.y || !groundTransform && MyTransform.position.y + realRadius < groundY)
        {
            BallPool.ReturnBall(this);
        }*/

        float cutoffHeight = visible;
        ballRenderer.material = new Material(destructionMaterial);
        for (; ; )
        {
            ballRigidbody.Sleep();
            ballRenderer.material.SetFloat("_CutoffHeight", cutoffHeight -= Time.deltaTime / destructionSpeed * Mathf.Abs(notVisible - visible));
            if(cutoffHeight <= notVisible)
            {
                BallPool.ReturnBall(this);
            }
            yield return null;
        }
    }

    protected void InitDestruction()
    {
        ballCollider.enabled = false;
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        MyTransform.localEulerAngles = Vector3.zero;
        StartCoroutine(Destroy());
    }
}

public class BallPool
{
    protected const string ballPrefabPath = "Prefabs/Sphere";
    protected static BallBehavior ballPrefab = Resources.Load<BallBehavior>(ballPrefabPath);

    protected static Queue<BallBehavior> balls = new Queue<BallBehavior>();

    public static BallBehavior GetBall(Vector3 position, Transform parent = null)
    {
        BallBehavior ball;
        if(balls.Count == 0)
        {
            ball = MakeNewBall();
        }
        else
        {
            ball = balls.Dequeue();
        }

        ZeroBall(ball);

        ball.MyTransform.position = position;
        ball.MyTransform.parent = parent;

        return ball;
    }
    public static void ZeroBall(BallBehavior ball)
    {
        ball.ballRenderer.material = ball.defaultMaterial;
        ball.MyGameObject.SetActive(true);
        ball.MyTransform.eulerAngles = Vector3.zero;
        ball.isGrounded = false;
        ball.groundTransform = null;
        ball.ballCollider.enabled = true;
        ball.ballRigidbody.velocity = Vector3.zero;
        ball.ballRigidbody.useGravity = true;
        ball.ballRigidbody.WakeUp();
        ball.currentTrack = null;
        ball.groudTimerRunning = true;
        ball.impulseTrack = null;
        ball.isDestroying = false;
        ball.lastTrack = null;
        ball.onTrack = false;
        ball.pathID = -1;
        ball.rollingSpeed = 1f;
        ball.timeElapsed = 0f;
        ball.timeLimit = 5f;
        ball.velocity = 0f;
    }
    public static void ReturnBall(BallBehavior returned)
    {
        returned.MyGameObject.SetActive(false);
        returned.MyTransform.parent = null;

        balls.Enqueue(returned);
    }
    protected static BallBehavior MakeNewBall()
    {
        return Object.Instantiate(ballPrefab, Vector3.zero, new Quaternion());
    }
}
