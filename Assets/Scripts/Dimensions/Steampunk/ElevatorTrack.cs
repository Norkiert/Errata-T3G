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
        const float rodThickHeight = 0.7951697f;
        const float rodThickScaleAddon = 1.0f / rodThickHeight / 2.0f;
    [SerializeField] public GameObject pistonRodThin;
        const float rodThinHeight = 0.6469156f;
        const float rodThinScaleAddon = 1.0f / rodThinHeight;
    [SerializeField] public GameObject pistonTrack;

    protected new void Awake()
    {
        base.Awake();
        heightAtStart = pistonTrack.transform.localPosition.y;
    }
    public override void RotateRight()
    {
        transform.Rotate(Vector3.up * 90);
    }
    public override void RotateLeft()
    {
        transform.Rotate(Vector3.down * 90);
    }
    public override void OnBallEnter(BallBehavior ball)
    {
        InitBallPath(ball);
        var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
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
            if (!isExtending && ballDistance <= Vector3.Distance(pistonTrack.transform.position, ball.transform.position) && pistonTrack.transform.localPosition.y == heightAtStart && normalMode)
            {
                BeginExtension();
            }
            else if (!isExtending)
            {
                ball.ballRigidbody.velocity = ball.rollingSpeed * rollingSpeed * ball.ballRigidbody.velocity.normalized;
                ballDistance = Vector3.Distance(pistonTrack.transform.position, ball.transform.position);
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
        transform.localPosition = GetLocalPosition();
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
            ballHeight = FirstBall.transform.position.y - pistonTrack.transform.position.y;
        }
        else
        {
            ballHeight = 0;
        }

        isExtending = true;
        for(; ; )
        {
            pistonRodThick.transform.localScale += Time.deltaTime * extensionSpeed * rodThickScaleAddon * Vector3.up;
            pistonRodThin.transform.localScale += Time.deltaTime * extensionSpeed * rodThinScaleAddon * Vector3.up;
            pistonTrack.transform.localPosition += Time.deltaTime * extensionSpeed * Vector3.up;
            foreach(var ball in balls)
            {
                ball.transform.position = new Vector3(ball.transform.position.x, pistonTrack.transform.position.y + ballHeight, ball.transform.position.z);
            }
            
            if(pistonTrack.transform.localPosition.y - heightAtStart >= elevatorHeight)
            { // track arrived at desired height
                isExtending = false;

                pistonRodThick.transform.localScale = new Vector3(pistonRodThick.transform.localScale.x, 1 + (elevatorHeight * rodThickScaleAddon), pistonRodThick.transform.localScale.z);
                pistonRodThin.transform.localScale = new Vector3(pistonRodThin.transform.localScale.x, 1 + (elevatorHeight * rodThinScaleAddon), pistonRodThin.transform.localScale.z);
                pistonTrack.transform.localPosition = new Vector3(pistonTrack.transform.localPosition.x, heightAtStart + elevatorHeight, pistonTrack.transform.localPosition.z);

                if (balls.Count == 0)
                {
                    StartCoroutine(Retract());
                }

                foreach (var ball in balls)
                {
                    ball.transform.position = new Vector3(ball.transform.position.x, pistonTrack.transform.position.y + ballHeight, ball.transform.position.z);

                    var moveVector = transform.rotation * (ball.rollingSpeed * rollingSpeed * ball.pathID switch
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
            pistonRodThick.transform.localScale += Time.deltaTime * extensionSpeed * rodThickScaleAddon * Vector3.down;
            pistonRodThin.transform.localScale += Time.deltaTime * extensionSpeed * rodThinScaleAddon * Vector3.down;
            pistonTrack.transform.localPosition += Time.deltaTime * extensionSpeed * Vector3.down;

            if(pistonTrack.transform.localPosition.y <= heightAtStart)
            { // track arrived at 0
                isRetracting = false;

                pistonRodThick.transform.localScale = new Vector3(pistonRodThick.transform.localScale.x, 1, pistonRodThick.transform.localScale.z);
                pistonRodThin.transform.localScale = new Vector3(pistonRodThin.transform.localScale.x, 1, pistonRodThin.transform.localScale.z);
                pistonTrack.transform.localPosition = new Vector3(pistonTrack.transform.localPosition.x, heightAtStart, pistonTrack.transform.localPosition.z);

                yield break;
            }

            yield return null;
        }
    }
}
