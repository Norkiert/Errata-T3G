using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AutoSpawnerTrack : BasicTrack
{
    public new const float length = 0.5f * 2;
    public new const float height = 0.12f * 2;
    public new const float width = 0.24f * 2;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/AutoSpawnerTrack.prefab";

    [SerializeField] protected float timeToSpawn;
    [SerializeField, ReadOnly] public float timeElapsed = 0f;

    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected BallBehavior prefab;

    [SerializeField] public bool locked = false;

    protected new void Awake()
    {
        base.Awake();
    }
    protected void Update()
    {
        if (!locked)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToSpawn)
            {
                timeElapsed = 0;
                SpawnBall();
            }
        }
    }
    protected void SpawnBall()
    {
        var ball = Instantiate(prefab, spawnPoint.position, new Quaternion());
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
        ball.pathID = 0;
        ball.ballRigidbody.velocity = rollingSpeed * ball.rollingSpeed * ball.ballRigidbody.velocity.normalized;
    }
    public override void OnBallStay(BallBehavior ball)
    {
        ball.ballRigidbody.velocity = rollingSpeed * ball.rollingSpeed * ball.ballRigidbody.velocity.normalized;
    }
    public override void OnBallExit(BallBehavior ball)
    {

    }
    public override void InitPos(TrackMapPosition tmp)
    {
        position = tmp;
        transform.localPosition = GetLocalPosition();
    }
}
