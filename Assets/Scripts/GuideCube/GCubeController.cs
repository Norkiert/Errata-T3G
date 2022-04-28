using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Pathfinding;

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

        [Header("Distances")]
        [SerializeField, Min(1)] private float maxDistFollowPlayer = 20f;
        [SerializeField, Min(1)] private float maxDistDialogue = 20f;
        
        private GCubeState currentState;
        private GCubeState defaultState;
        private readonly List<GCubeState> nextStates = new List<GCubeState>();

        public PlayerController Player { get; private set; }
        private HighlightableOnSelect highlightable;
        private Clickable clickable;

        public static GCubeController Instance { get; private set; } = null;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"{nameof(GCubeController)} already exist!");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Player = FindObjectOfType<PlayerController>();
            highlightable = GetComponent<HighlightableOnSelect>();
            clickable = GetComponent<Clickable>();

            defaultState = new GCSIdle(this);
        }

        private void Start()
        {
            SetState(new GCSGoTo(this, NearestPointPosition)); 
        }

        private void OnEnable() => clickable.OnClick += OnClicked;
        private void OnDisable() => clickable.OnClick -= OnClicked;

        private void Update()
        {
            currentState?.Update();
        }

        public void SetState(GCubeState state) => SetState(new List<GCubeState> { state });
        public void SetState(GCubeState state, GCubeState nextState) => SetState(new List<GCubeState> { state, nextState });
        public void SetState(List<GCubeState> followinStates)
        {
            if (!CheckStateList(followinStates))
                return;

            GCubeState stateToStart = followinStates[0];

            KillCurrentState();

            nextStates.Clear();
            for (int i = 1; i < followinStates.Count; i++)
                nextStates.Add(followinStates[i]);

            StartState(stateToStart);
        }

        public void EndCurrentState()
        {
            KillCurrentState();

            if (nextStates.Count > 0)
            {
                GCubeState stateToSet = nextStates[0];
                nextStates.RemoveAt(0);
                StartState(stateToSet);
            }
            else
                SetState(defaultState);
        }

        private void StartState(GCubeState state)
        {
            Debug.Log($"GuideCube: Select state {state.GetType()}");
            currentState = state;
            currentState.Start();
        }
        private void KillCurrentState()
        {
            SetRotating(false);
            SetVerticalOscylation(false);
            CancelTarget();
            SetHightlithing(false);

            currentState?.End();

            currentState = null;
        }

        private bool CheckStateList(List<GCubeState> states)
        {
            if (states == null)
            {
                Debug.LogWarning($"{name}: Attepted set null state!");
                return false;
            }

            for (int i = 0; i < states.Count; i++)
                if (states[i] == null)
                {
                    Debug.LogWarning($"{name}: found null state on followinStates list!");
                    return false;
                }

            if (states.Count == 0)
            {
                Debug.LogWarning($"{name}: Attepted set empty state list!");
                return false;
            }

            return true;
        }

        public Vector3 NearestPointPosition => Pathfinder.FindClosestPoint(Position)?.Position ?? Position;
        public Vector3 Position => transform.position;


        public float MaxDistFollowPlayer => maxDistFollowPlayer;
        public float MaxDistDialogue => maxDistDialogue;
        public GCubeState CurrentState => currentState;


        #region -target following-

        private Vector3 target;
        private IEnumerator targetFollowing = null;
        public void GoToTarget(Vector3 target, float avaDistanceToTarget)
        {
            if (Vector3.Distance(this.target, target) < 0.1f)
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
            List<Point> path = null;

            Vector3 oldTarget = transform.position;

            Vector3 fixedTarget = target;
            float validDistance = avaDistanceToTarget;

            int currentPathIndex = 0;
            while (true)
            {
                // update path aftar target changed
                if (path == null || Vector3.Distance(target, oldTarget) > 1)
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

                    validDistance = Mathf.Max(avaDistanceToTarget - Vector3.Distance(fixedTarget, target), 0.1f);

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

        public void SetHightlithing(bool value)
        {
            if (highlightable)
                highlightable.enabled = value;
        }

        private void OnClicked() => currentState?.OnClicked();
    }
}