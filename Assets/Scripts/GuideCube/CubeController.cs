using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using PathFinding;

public class CubeController : MonoBehaviour
{
    public enum CubeStates {None, Idle, FlyToTarget, Questions}

    [SerializeField, ReadOnly] private CubeStates currentState = CubeStates.None;

    [Header("Idle")]
    [SerializeField] private float rotateSpeed = 0.9f;
    [SerializeField] private float timeToChangeRotation = 2f;
    [SerializeField] private float flyingUpTime = 1f;
    [SerializeField] private float flyingUpDistance = 1f;

    [Header("Flying attributes")]
    [SerializeField] private float flyingSpeed = 5f;
    [SerializeField, Min(1)] private float maxDistanceFromPlayer = 20f;
    [SerializeField] private bool roateDuringFly = true;

    private Vector3 target;
    private IEnumerator rotating;
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SwitchState(CubeStates.Idle);

        StartCoroutine(DistanceCheck());
    }

    private void SwitchState(CubeStates newState)
    {
        if (newState == currentState)
            return;

        //Debug.Log($"Select state {newState}");
        bool rotate = false;

        currentState = newState;
        switch (newState)
        {
            case CubeStates.Idle:
                {
                    StartCoroutine(Idle());
                    rotate = true;
                    break;
                }
            case CubeStates.FlyToTarget:
                {
                    StartCoroutine(FlyToTarget());
                    if (roateDuringFly)
                        rotate = true;
                    break;
                }
            default:
                {
                    Debug.LogWarning($"{newState} State was not declared!");
                    break;
                }
        }

        if (rotate != (rotating != null))
        {
            if (rotate)
            {
                rotating = Rotate();
                StartCoroutine(rotating);
            }
            else
            {
                StopCoroutine(rotating);
                rotating = null;
            }
                
        }
    }

    /// <summary>
    /// Coroutine changing transform of Cube, to reach given coordinates
    /// </summary>
    /// <param name="pointsToGo">
    /// PointsToGo is a list of Points which contains all points calculated by pathfinding scripts, that Cube should travel to, to finally reach given target
    /// </param>
    public IEnumerator FlyToTarget()
    {
        List<Point> path = new List<Point>();

        Vector3 oldTarget = transform.position;

        Vector3 fixedTarget = target;
        float validDistance = maxDistanceFromPlayer;

        int currentPathIndex = 0;
        while (currentState == CubeStates.FlyToTarget)
        {
            // update path aftar target changed
            if (Vector3.Distance(target, oldTarget) > 2)
            {
                //Debug.Log($"Reques new path target: {fixedTarget} old target: {oldTarget}");
                oldTarget = target;

                (path, fixedTarget) = Pathfinding.FindPath(transform.position, target);
                validDistance = Mathf.Max(maxDistanceFromPlayer - Vector3.Distance(fixedTarget, target), 1);
                
                //Debug.Log($"{path.Count} {Vector3.Distance(path[0].Position, target)} {Vector3.Distance(path[path.Count - 1].Position, target)}");
                if (path.Count > 1 && Vector3.Distance(path[0].Position, fixedTarget) > Vector3.Distance(path[1].Position, fixedTarget))
                    currentPathIndex = 1;
                else
                    currentPathIndex = 0;

                // debug
                Vector3 lastPos = transform.position;
                for (int i = currentPathIndex; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(lastPos, path[i].Position, Color.red, 3);
                    lastPos = path[i].Position;
                }
                Debug.DrawLine(lastPos, fixedTarget, Color.red, 3);
            }

            float distanceDisFrame = Time.deltaTime * flyingSpeed;
            float distanceToCurrentPoint = Vector3.Distance(transform.position, path[currentPathIndex].Position);

            // check if achieve target
            if (Vector3.Distance(transform.position, fixedTarget) < Mathf.Max(distanceDisFrame, validDistance))
            {
                SwitchState(CubeStates.Idle);
                yield break;
            }

            // change index when is near to current target point
            if (distanceDisFrame > distanceToCurrentPoint)
            {
                currentPathIndex++;

                if (currentPathIndex == path.Count)
                {
                    //Debug.LogWarning($"{name}: End of path!");
                    SwitchState(CubeStates.Idle);
                    yield break;
                }

                continue;
            }

            Vector3 direction = (path[currentPathIndex].Position - transform.position).normalized;
            transform.position += direction * distanceDisFrame;

            yield return null;
        }
    }

    /// <summary>
    /// Coroutine that provides upside-down movement of Cube in Idle state
    /// </summary>
    private IEnumerator Idle()
    {
        float moveTime = 0;
        bool dir = true;
        Tween move = null;

        while (currentState == CubeStates.Idle)
        {
            moveTime -= Time.deltaTime;
            if (moveTime <= 0)
            {
                moveTime = flyingUpTime;
                move = transform.DOMoveY(transform.position.y + (dir ? flyingUpDistance : -flyingUpDistance), flyingUpTime).SetEase(Ease.InQuad);
                dir = !dir;
            }

            yield return null;
        }

        if (move != null)
            move.Kill();
    }

    private IEnumerator Rotate()
    {
        float rotationTimer = 0;
        Quaternion targetRotation = transform.rotation;

        while (true)
        {
            rotationTimer -= Time.deltaTime;
            if (rotationTimer <= 0)
            {
                targetRotation = Quaternion.Euler(new Vector3(Random.Range(-720, 720), Random.Range(-720, 720), Random.Range(-720, 720)));
                rotationTimer = timeToChangeRotation;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            yield return null;
        }
    }

    /// <summary>
    /// Coroutine DistanceCheck checks distance between player and Cube each second, and call function to move cube to player if that distance is higher than given
    /// </summary>
    private IEnumerator DistanceCheck()
    {
        while (true)
        {
            if (Vector3.Distance(target, player.transform.position) > maxDistanceFromPlayer + 1 + flyingUpDistance)
            {
                target = player.transform.position;
                SwitchState(CubeStates.FlyToTarget);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
