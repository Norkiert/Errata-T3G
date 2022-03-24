using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using PathFinding;

public class CubeController : MonoBehaviour
{
    public enum CubeStates {None, Idle, Flying, Questions}

    [SerializeField, ReadOnly] private CubeStates actualState = CubeStates.None;

    [Header("Idle")]
    [SerializeField] private float rotateSpeed = 0.9f;
    [SerializeField] private float timeToChangeRotation = 2f;
    [SerializeField] private float flyingUpTime = 1f;
    [SerializeField] private float flyingUpDistance = 1f;

    [Header("Flying attributes")]
    [SerializeField] private float flyingSpeed = 5f;
    [SerializeField] private float maxDistanceFromPlayer = 20f;
    [SerializeField] private bool enableRotatingWhileFlying = true;


    private List<Point> path = new List<Point>();
    private PlayerController player;
    private Vector3 PortalPoint;

    private float timer = 0f;
    private Quaternion rotating;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SwitchState(CubeStates.Idle);
        StartCoroutine(DistanceCheck());
    }

    private void SwitchState(CubeStates newState)
    {
        if (newState == actualState)
            return;
        switch(newState)
        {
            case CubeStates.Idle:
                {
                    StartCoroutine(SelfMoving());
                    break;
                }
            case CubeStates.Flying:
                {
                    StopCoroutine(SelfMoving());
                    break;
                }
            case CubeStates.Questions:
                {
                    //QuestionsCode
                    break;
                }
            default:
                {
                    Debug.LogWarning("State switch error occur");
                    break;
                }
        }
        actualState = newState;
    }

    /// <summary>
    /// Function that checks how Cube should reach given position, then it find path and in loop call GoToPoint function to reach given target
    /// </summary>
    /// <param name="target">
    /// Target is vector3 that cube should reach at the end of function
    /// </param>
    private void FindPathAndReachTarget(Vector3 target)
    {
        if (actualState == CubeStates.Flying)
        {
            StopCoroutine(GoToPoint(new List<Point>())) ;
        }
        else
        {
            SwitchState(CubeStates.Flying);
        }
        path = Pathfinding.FindPath(transform.position, target);
        if(path!=null)
        {
            StartCoroutine(GoToPoint(path));
        }
    }

    /// <summary>
    /// Coroutine changing transform of Cube, to reach given coordinates
    /// </summary>
    /// <param name="pointsToGo">
    /// PointsToGo is a list of Points which contains all points calculated by pathfinding scripts, that Cube should travel to, to finally reach given target
    /// </param>
    public IEnumerator GoToPoint(List<Point> pointsToGo)
    {
        SwitchState(CubeStates.Flying);
        for (int i =0; i<pointsToGo.Count;i++)
        {
            while (Vector2.Distance(new Vector2(pointsToGo[i].transform.position.x, pointsToGo[i].transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 0.3f)
            {
                transform.position = new Vector3(Vector3.MoveTowards(transform.position, pointsToGo[i].transform.position, Time.deltaTime * flyingSpeed).x,transform.position.y, Vector3.MoveTowards(transform.position, pointsToGo[i].transform.position, Time.deltaTime * flyingSpeed).z);
                yield return null;
            }
        }
        SwitchState(CubeStates.Idle);
    }
    /// <summary>
    /// Coroutine that provides upside-down movement of Cube in Idle state
    /// </summary>
    private IEnumerator SelfMoving()
    {
        bool direction = false;
        while(true)
        {
            if(direction)
            {
                transform.DOMoveY(transform.position.y + flyingUpDistance, flyingUpTime);
            }else
            {
                transform.DOMoveY(transform.position.y - flyingUpDistance, flyingUpTime);
            }
            yield return new WaitForSeconds(flyingUpTime);
            direction = !direction;
        }
    }
    /// <summary>
    /// Coroutine DistanceCheck checks distance between player and Cube each second, and call function to move cube to player if that distance is higher than given
    /// </summary>
    private IEnumerator DistanceCheck()
    {
        while(true)
        {
            if(actualState != CubeStates.Flying)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > maxDistanceFromPlayer)
                {
                    FindPathAndReachTarget(player.transform.position);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Update is only for rotating cube
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer<timeToChangeRotation)
        {
            if (!enableRotatingWhileFlying && actualState == CubeStates.Flying) { }
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, rotating, Time.deltaTime * rotateSpeed);
        }else
        {
            rotating = Quaternion.Euler(new Vector3(Random.Range(-720, 720), Random.Range(-720, 720), Random.Range(-720, 720)));
            timer = 0f;
        }
    }
}
