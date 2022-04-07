using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Pathfinding;
using GuideCube.States;

namespace GuideCube
{
    public class GCubeController : MonoBehaviour
    {
        [Header("Idle")]
        [SerializeField] private float rotateSpeed = 0.9f;
        [SerializeField] private float timeToChangeRotation = 2f;
        [SerializeField] private float flyingUpTime = 1f;
        [SerializeField] private float flyingUpDistance = 1f;

        [Header("Flying attributes")]
        [SerializeField] private float flyingSpeed = 5f;
        [SerializeField, Min(1)] private float maxDistanceFromPlayer = 20f;

        private GCubeState currentState;
        private GCubeState defaultState;
        private readonly List<GCubeState> nextStates = new List<GCubeState>();

        private PlayerController player;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
            defaultState = new GCSFollowPlayer(this, player, maxDistanceFromPlayer);
            SetState(defaultState);
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void SetState(GCubeState state, List<GCubeState> nextStates=null)
        {
            if (state == null)
            {
                Debug.LogWarning($"{name}: Attepted set null state!");
                return;
            }

            Debug.Log($"Select state {state.GetType()}");

            KillCurrentState();

            this.nextStates.Clear();
            if (nextStates != null)
                this.nextStates.AddRange(nextStates);

            currentState = state;

            currentState.Start();
        }
        private void KillCurrentState()
        {
            SetRotating(false);
            SetVerticalOscylation(false);
            CancelTarget();

            currentState = null;
        }
        public void EndCurrentState()
        {
            KillCurrentState();

            GCubeState stateToSet = defaultState;

            if (nextStates.Count > 0)
            {
                stateToSet = nextStates[0];
                nextStates.RemoveAt(0); 
            }

            SetState(stateToSet);
        }


        #region -target following-

        private Vector3 target;
        private IEnumerator targetFollowing = null;
        public void GoToTarget(Vector3 target, float avaDistanceToTarget)
        {
            if (Vector3.Distance(this.target, target) < 1)
                return;

            this.target = target;

            if (avaDistanceToTarget < 0)
                avaDistanceToTarget = 0;

            if (targetFollowing == null)
            {
                targetFollowing = FollowTarget(avaDistanceToTarget);
                StartCoroutine(targetFollowing);
            }
        }
        public void CancelTarget()
        {
            if (targetFollowing != null)
            {
                StopCoroutine(targetFollowing);
                targetFollowing = null;
            }

            target = Vector3.zero;
        }
        public bool IsOnTheWay => targetFollowing != null;
        private IEnumerator FollowTarget(float avaDistanceToTarget)
        {
            List<Point> path = new List<Point>();

            Vector3 oldTarget = transform.position;

            Vector3 fixedTarget = target;
            float validDistance = avaDistanceToTarget;

            int currentPathIndex = 0;
            while (true)
            {
                // update path aftar target changed
                if (Vector3.Distance(target, oldTarget) > 2)
                {
                    //Debug.Log($"Reques new path target: {fixedTarget} old target: {oldTarget}");
                    oldTarget = target;

                    (path, fixedTarget) = Pathfinder.FindPath(transform.position, target);

                    if (path == null)
                    {
                        Debug.LogWarning($"{name}: Dont found path!");
                        CancelTarget();
                        yield break;
                    }

                    validDistance = Mathf.Max(avaDistanceToTarget - Vector3.Distance(fixedTarget, target), 1);

                    currentPathIndex = 0;

                    // fix retreat to the first point if it is unnecessary
                    if (path.Count > 1 && !(path[0] is PointWithPortal portalPoint && portalPoint.ConnectedPortalPoint == path[1]))
                    {
                        const float dirError = 0.2f;
                        Vector3 firstDir = (path[0].Position - transform.position).normalized;
                        Vector3 secondDir = (path[1].Position - path[0].Position).normalized;
                        Vector3 toTargetDir = (fixedTarget - transform.position).normalized;
                        //Debug.Log($"firstDir: {firstDir} secondDir: {secondDir} dif= {Vector3.Distance(firstDir, secondDir)} < {dirError} ?");
                        if (Vector3.Distance(firstDir, toTargetDir) > dirError && Vector3.Distance(-firstDir, secondDir) < dirError)
                            currentPathIndex = 1;
                    }

                    // debug
                    {
                        /*
                    Debug.Log($"path (started from {transform.position} index: {currentPathIndex}):");
                    for (int i = 0; i < path.Count; i++)
                        Debug.Log($"{path[i].name} {path[i].Position}");
                    Debug.Log($"fixed target: {fixedTarget}");
                    */
                        Vector3 lastPos = transform.position;
                        for (int i = currentPathIndex; i < path.Count - 1; i++)
                        {
                            Debug.DrawLine(lastPos, path[i].Position, Color.red, 3);
                            lastPos = path[i].Position;

                        }
                        Debug.DrawLine(lastPos, fixedTarget, Color.red, 3);
                    }
                }

                float distanceDisFrame = Time.deltaTime * flyingSpeed;

                // check if achieve target
                if (Vector3.Distance(transform.position, fixedTarget) < Mathf.Max(distanceDisFrame, validDistance))
                {
                    CancelTarget();
                    yield break;
                }

                // change index when is near to next path point
                float distanceToNextPathPoint = Vector3.Distance(transform.position, path[currentPathIndex].Position);
                if (distanceDisFrame > distanceToNextPathPoint)
                {
                    currentPathIndex++;

                    // check if has avaialbe next path points
                    if (currentPathIndex == path.Count)
                    {
                        Debug.LogWarning($"{name}: End of path!");
                        CancelTarget();
                        yield break;
                    }

                    // use portal
                    if (path[currentPathIndex - 1] is PointWithPortal pointWithPortal && path[currentPathIndex] == pointWithPortal.ConnectedPortalPoint)
                    {
                        //Debug.Log("Using portal");

                        // use in offset
                        if (pointWithPortal.PortalOffset != null)
                        {
                            Vector3 localTarget = pointWithPortal.PortalOffset.position;
                            float distance = Vector3.Distance(transform.position, localTarget);
                            float ddf = Time.deltaTime * flyingSpeed;
                            while (distance > ddf)
                            {
                                distance = Vector3.Distance(transform.position, localTarget);
                                ddf = Time.deltaTime * flyingSpeed;

                                Vector3 dir = (localTarget - transform.position).normalized;
                                transform.position += dir * ddf;

                                yield return null;
                            }

                            yield return null;
                            transform.position = localTarget;
                            yield return null;
                        }

                        // use out offset
                        if (pointWithPortal.ConnectedPortalPoint.PortalOffset != null)
                        {
                            // teleport
                            transform.position = pointWithPortal.ConnectedPortalPoint.PortalOffset.position;

                            Vector3 localTarget = pointWithPortal.ConnectedPortalPoint.Position;
                            float distance = Vector3.Distance(transform.position, localTarget);
                            float ddf = Time.deltaTime * flyingSpeed;
                            while (distance > ddf)
                            {
                                distance = Vector3.Distance(transform.position, localTarget);
                                ddf = Time.deltaTime * flyingSpeed;

                                Vector3 dir = (localTarget - transform.position).normalized;
                                transform.position += dir * ddf;

                                yield return null;
                            }

                            yield return null;
                            transform.position = localTarget;
                            yield return null;
                        }
                        // teleport
                        else
                            transform.position = pointWithPortal.ConnectedPortalPoint.Position;
                    }

                    continue;
                }

                // move
                Vector3 direction = (path[currentPathIndex].Position - transform.position).normalized;
                transform.position += direction * distanceDisFrame;

                yield return null;
            }
        }

        #endregion

        #region -vertical oscilation-

        private Sequence veritcalOscilation = null;
        public void SetVerticalOscylation(bool value)
        {
            if (value == (veritcalOscilation != null))
                return;

            if (value)
            {
                veritcalOscilation = DOTween.Sequence()
                        .Append(transform.DOMoveY(transform.position.y + flyingUpDistance, flyingUpTime).SetEase(Ease.InOutSine))
                        .SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                veritcalOscilation.Kill();
                veritcalOscilation = null;
            }   
        }

        #endregion

        #region -rotating-

        private IEnumerator rotating = null;
        public void SetRotating(bool value)
        {
            if (value == (rotating != null))
                return;

            if (value)
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

        #endregion
    }
}