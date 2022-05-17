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
    [SerializeField] protected float destructionSpeed = 0.005f;
    [SerializeField] protected float groundY;

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

    protected void Destroy()
    {
        MyTransform.position = MyTransform.position + Vector3.down * destructionSpeed;
        if (groundTransform && MyTransform.position.y + realRadius < groundTransform.position.y || !groundTransform && MyTransform.position.y + realRadius < groundY)
        {
            BallPool.ReturnBall(this);
        }
    }

    protected void InitDestruction()
    {
        ballCollider.enabled = false;
        ballRigidbody.useGravity = false;
    }
}

public class BallPool
{
    protected const string ballPrefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/Sphere.prefab";
    protected static BallBehavior ballPrefab = AssetDatabase.LoadAssetAtPath<BallBehavior>(ballPrefabPath);

    protected static Queue<BallBehavior> balls = new Queue<BallBehavior>();
    public static float BallsCount { get; protected set; }

    public static BallBehavior GetBall(Vector3 position, Transform parent = null)
    {
        BallBehavior ball;
        if(balls.Count == 0)
        {
            ball = MakeNewBall();
            ++BallsCount;
            Debug.Log(BallsCount);
        }
        else
        {
            ball = balls.Dequeue();
        }

        // zero the ball
        ball.MyGameObject.SetActive(true);
        ball.MyTransform.position = position;
        ball.MyTransform.parent = parent;
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

        return ball;
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
