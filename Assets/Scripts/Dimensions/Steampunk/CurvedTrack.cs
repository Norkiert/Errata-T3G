using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedTrack : BasicTrack
{
    public new const float length = (ModelTrack.length + ModelTrack.width) / 2;
    public new const float height = ModelTrack.height;
    public new const float width = length;
    public new const string prefabPath = "Assets/Art/Dimensions/Steampunk/Prefabs/CurvedTrack.prefab";


    //sory krisu to do przeszkadzajek
    private CharacterController player;
    private ParticleSystem sparks;
    private GameObject sparkGO;
    private Vector3 impact = Vector3.zero;
    private Camera playerCam;
    [SerializeField] private float pushForce = 20f;

    [SerializeField] public Transform rotationPoint;

    public new void Awake()
    {
        sparkGO = GameObject.Find("TrackParticles");
        if(sparkGO!=null)
            sparks = sparkGO.GetComponent<ParticleSystem>();
        base.Awake();
    }
    private void Start()
    {
        GameObject cam = GameObject.Find("PlayerCamera");
        if(cam!=null)
            playerCam = cam.GetComponent<Camera>();
        player = FindObjectOfType<CharacterController>();
    }
    public override void RotateRight()
    {
        MyTransform.Rotate(Vector3.up * 90);
        if(sparkGO!=null)
        {
            if (!SaveManager.IsLevelFinished(Dimension.Electrical))
            {
                if (((Random.value * 10000000) % 10 * (Random.value * 10000000) % 10) % 9 > 6)
                {
                    sparks.transform.position = (this.transform.position + new Vector3(0.15f, -0.1f, -0.15f));
                    sparks.Play();
                    AddImpact(playerCam.transform.forward * -1, pushForce);
                }
            }
        }
    }
    public override void RotateLeft()
    {
        MyTransform.Rotate(Vector3.up * -90);
        if(sparkGO != null)
        {
            if (!SaveManager.IsLevelFinished(Dimension.Electrical))
            {
                if (((Random.value * 10000000) % 10 * (Random.value * 10000000) % 10) % 9 > 6)
                {
                    sparks.transform.position = (this.transform.position + new Vector3(0.15f, -0.1f, -0.15f));
                    sparks.Play();
                    AddImpact(playerCam.transform.forward * -1, pushForce);
                }
            }
        }
    }
    public override void OnBallEnter(BallBehavior ball)
    {
        InitBallPath(ball);
        var deltaX = rotationPoint.position.x - ball.MyTransform.position.x;
        var deltaZ = rotationPoint.position.z - ball.MyTransform.position.z;

        var angle = Quaternion.Euler(0, Mathf.Atan2(deltaZ, deltaX) * Mathf.Rad2Deg, 0) * MyTransform.rotation;

        var moveVector = angle * (Quaternion.Inverse(MyTransform.rotation) * (ball.pathID switch
        {
            0 => Vector3.forward,
            1 => Vector3.back,
            _ => Vector3.zero
        })) * rollingSpeed * ball.rollingSpeed;
        moveVector.x *= -1;

        ball.ballRigidbody.velocity = moveVector;
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
        MyTransform.localPosition = GetLocalPosition();
    }

    private void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        impact += dir * force;
    }

    private void Update()
    {
        if (impact.magnitude > 0.2) player.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
}
