using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ClickSpawnerTrack : BasicTrack
{
    public new const float length = 0.5f * 2;
    public new const float height = 0.12f * 2;
    public new const float width = 0.24f * 2;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/ClickSpawnerTrack.prefab";

    [SerializeField] protected GameObject ballPrefab;
    [SerializeField] protected Material defaultMaterial;

    [SerializeField] protected Material shaderMaterial;
    [SerializeField] protected float notVisible = -1.44f;
    [SerializeField] protected float visible = 1.78f;
    [SerializeField] protected float spawningSpeed = 1f;

    [SerializeField] protected Transform spawnPoint;

    [SerializeField, ReadOnly] protected bool isSpawning = false;
    [SerializeField, ReadOnly] protected BallBehavior currentBall;
    protected Renderer ballRenderer;
    [SerializeField, ReadOnly] protected float cutoffHeight = 0f;

    protected new void Awake()
    {
        base.Awake();
        OnClick -= RotateRight;
        OnClick += InitBallSpawn;
    }
    protected void Update()
    {
        if (isSpawning && cutoffHeight >= visible)
        {
            StopAllCoroutines();

            ballRenderer.material.SetFloat("_CutoffHeight", visible);

            currentBall.ballRigidbody.WakeUp();
            ballRenderer.material = defaultMaterial;

            currentBall = null;
            cutoffHeight = 0f;
            isSpawning = false;
            ballRenderer = null;
        }
    }
    protected void InitBallSpawn()
    {
        if (!isSpawning)
        {
            isSpawning = true;

            currentBall = Instantiate(ballPrefab, spawnPoint.transform.position, new Quaternion()).GetComponent<BallBehavior>();
            currentBall.ballRigidbody.Sleep();

            ballRenderer = currentBall.GetComponent<Renderer>();
            ballRenderer.material = new Material(shaderMaterial);
            ballRenderer.material.SetFloat("_CutoffHeight", notVisible);
            cutoffHeight = notVisible;

            StartCoroutine(DissolveEffect());
        }
    }
    protected IEnumerator DissolveEffect()
    {
        for(; ; )
        {
            ballRenderer.material.SetFloat("_CutoffHeight", cutoffHeight += Time.deltaTime / spawningSpeed * Mathf.Abs(notVisible - visible));
            yield return null;
        }
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