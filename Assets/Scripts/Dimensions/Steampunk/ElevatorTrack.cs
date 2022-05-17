using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ElevatorTrack : BasicTrack
{
    public new const float length = 0.5f * 2;
    public new const float height = 1.316739f;
    public new const float width = 0.24f * 2;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/ElevatorTrack.prefab";
    
    [SerializeField] public float elevatorHeight = 1f;

    [SerializeField] public float extensionSpeed = 0.5f;
    [SerializeField][ReadOnly] public bool isExtending = false;
    [SerializeField][ReadOnly] public bool isRetracting = false;
    protected float heightAtStart;

    [SerializeField] public bool normalMode = true;

    protected float ballHeight;
    protected float ballDistance = float.MaxValue;

    [SerializeField] public GameObject pistonRodThick;
    protected Transform pistonRodThickT;
        const float rodThickHeight = 0.7951697f;
        const float rodThickScaleAddon = 1.0f / rodThickHeight / 2.0f;
    [SerializeField] public GameObject pistonRodThin;
    protected Transform pistonRodThinT;
        const float rodThinHeight = 0.6469156f;
        const float rodThinScaleAddon = 1.0f / rodThinHeight;
    [SerializeField] public GameObject pistonTrack;
    protected Transform pistonTrackT;

    protected new void Awake()
    {
        base.Awake();

        pistonTrackT = pistonTrack.transform;
        pistonRodThinT = pistonRodThin.transform;
        pistonRodThickT = pistonRodThick.transform;

        heightAtStart = pistonTrackT.localPosition.y;
    }
    public override void RotateRight()
    {
        MyTransform.Rotate(Vector3.up * 90);
    }
    public override void RotateLeft()
    {
        MyTransform.Rotate(Vector3.down * 90);
    }
    public override void OnBallEnter(BallBehavior ball)
    {
        InitBallPath(ball);
        var moveVector = MyTransform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
        {
            0 => Vector3.forward,
            1 => Vector3.back,
            _ => Vector3.zero
        });
        ball.ballRigidbody.velocity = moveVector;
    }
    public override void OnBallStay(BallBehavior ball)
    {
        if (!isRetracting)
        {
            // initialize upwards extension
            if (!isExtending && ballDistance <= Vector3.Distance(pistonTrackT.position, ball.MyTransform.position) && pistonTrackT.localPosition.y == heightAtStart && normalMode)
            {
                BeginExtension();
            }
            else if (!isExtending)
            {
                ball.ballRigidbody.velocity = ball.rollingSpeed * rollingSpeed * ball.ballRigidbody.velocity.normalized;
                ballDistance = Vector3.Distance(pistonTrackT.position, ball.MyTransform.position);
            }
            else
            {
                ball.ballRigidbody.velocity = Vector3.zero;
            }
        }
    }
    public override void OnBallExit(BallBehavior ball)
    {
        if (!isRetracting && !isExtending && balls.Count == 0)
        {
            StartCoroutine(Retract());
        }
    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        MyTransform.localPosition = GetLocalPosition();
    }
    public void BeginExtension()
    {
        StartCoroutine(Extend());
    }
    public IEnumerator Extend()
    {
        if (isRetracting)
        {
            yield break;
        }

        if (FirstBall)
        {
            ballHeight = FirstBall.MyTransform.position.y - pistonTrackT.position.y;
        }
        else
        {
            ballHeight = 0;
        }

        isExtending = true;
        for(; ; )
        {
            pistonRodThickT.localScale += Time.deltaTime * extensionSpeed * rodThickScaleAddon * Vector3.up;
            pistonRodThinT.localScale += Time.deltaTime * extensionSpeed * rodThinScaleAddon * Vector3.up;
            pistonTrackT.localPosition += Time.deltaTime * extensionSpeed * Vector3.up;
            foreach(var ball in balls)
            {
                ball.MyTransform.position = new Vector3(ball.MyTransform.position.x, pistonTrackT.position.y + ballHeight, ball.MyTransform.position.z);
            }
            
            if(pistonTrackT.localPosition.y - heightAtStart >= elevatorHeight)
            { // track arrived at desired height
                isExtending = false;

                pistonRodThickT.localScale = new Vector3(pistonRodThickT.localScale.x, 1 + (elevatorHeight * rodThickScaleAddon), pistonRodThickT.localScale.z);
                pistonRodThinT.localScale = new Vector3(pistonRodThinT.localScale.x, 1 + (elevatorHeight * rodThinScaleAddon), pistonRodThinT.localScale.z);
                pistonTrackT.localPosition = new Vector3(pistonTrackT.localPosition.x, heightAtStart + elevatorHeight, pistonTrackT.localPosition.z);

                if (balls.Count == 0)
                {
                    StartCoroutine(Retract());
                }

                foreach (var ball in balls)
                {
                    ball.MyTransform.position = new Vector3(ball.MyTransform.position.x, pistonTrackT.position.y + ballHeight, ball.MyTransform.position.z);

                    var moveVector = MyTransform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
                    {
                        0 => Vector3.forward,
                        1 => Vector3.back,
                        _ => Vector3.zero
                    });
                    ball.ballRigidbody.velocity = moveVector;
                }

                yield break;
            }

            yield return null;
        }
    }
    public IEnumerator Retract()
    {
        if (isExtending)
        {
            yield break;
        }

        isRetracting = true;
        for(; ; )
        {
            pistonRodThickT.localScale += Time.deltaTime * extensionSpeed * rodThickScaleAddon * Vector3.down;
            pistonRodThinT.localScale += Time.deltaTime * extensionSpeed * rodThinScaleAddon * Vector3.down;
            pistonTrackT.localPosition += Time.deltaTime * extensionSpeed * Vector3.down;

            if(pistonTrackT.localPosition.y <= heightAtStart)
            { // track arrived at 0
                isRetracting = false;

                pistonRodThickT.localScale = new Vector3(pistonRodThickT.localScale.x, 1, pistonRodThickT.localScale.z);
                pistonRodThinT.localScale = new Vector3(pistonRodThinT.localScale.x, 1, pistonRodThinT.localScale.z);
                pistonTrackT.localPosition = new Vector3(pistonTrackT.localPosition.x, heightAtStart, pistonTrackT.localPosition.z);

                yield break;
            }

            yield return null;
        }
    }
}
