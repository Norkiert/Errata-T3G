using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PathFinding
{
    public class Pathfinding : MonoBehaviour
    {
        private const float distanceToEndMtltiplier = 0.4f; // min 0

        [SerializeField, Min(1)] private float maxNeighbourDistance = 60f;
        [SerializeField] private bool showConnections = false;

        private const int defultPathBlockers = 1 << 3;
        [SerializeField] private LayerMask pathBlockers = defultPathBlockers;

        private static Point[] allPoints;


        [Button("Update Points")]
        private void ButtonUdatePoints() => UpdatePoints();


        private static Pathfinding instance;
        private static Pathfinding Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Pathfinding>();
                return instance;
            }
        }

        public static float MaxNeighbourDistance => Instance.maxNeighbourDistance;
        public static LayerMask PathBlockers => Instance == null ? (LayerMask)defultPathBlockers : Instance.pathBlockers;

        public static bool ShowConnections => Instance == null ? false : Instance.showConnections;

        private void Start()
        {
            UpdatePoints();
        }

        public static void UpdatePoints()
        {
            allPoints = FindObjectsOfType<Point>();
            foreach (Point point in allPoints)
                point.FindNeighbours(allPoints);
        }

        public static (List<Point> path, Vector3 fixedTarget) FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            UpdatePoints();


            if (allPoints.Length <= 1)
                return (null, targetPosition);


            (Point start, _) = FindClosestLine(startPosition);
            (Point end, Point endSecondPoint) = FindClosestLine(targetPosition);


            if (start == null || end == null)
            {
                Debug.LogWarning($"Dont found {(start == null ? "start" : "end")} path! (start: {startPosition} target: {targetPosition})");
                return (null, targetPosition);
            }   


            if (start == end && endSecondPoint != null)
            {
                // swap end and endSecondPoint
                Point t = end;
                end = endSecondPoint;
                endSecondPoint = t;
            }

            List<Point> path = CreatePath(start, end);


            if (path == null)
                return (null, targetPosition);


            if (endSecondPoint != null && !path.Contains(endSecondPoint))
                path.Add(endSecondPoint);


            // fix target position
            if (path.Count == 1)
                targetPosition = path[path.Count - 1].Position;
            else if (path.Count > 1)
            {
                Vector3 previousPoint = path[path.Count - 2].Position;
                Vector3 endPoint = path[path.Count - 1].Position;
                Vector3 dir = endPoint - previousPoint;
                Vector3 step = dir.normalized * 0.5f;
                Vector3 fixedTarget = previousPoint;
                float closesetDistance = Vector3.Distance(fixedTarget, targetPosition);
                int steps = Mathf.FloorToInt(dir.magnitude / step.magnitude);
                for (int i = 0; i < steps; i++)
                {
                    fixedTarget += step;
                    float dist = Vector3.Distance(fixedTarget, targetPosition);
                    if (dist > closesetDistance)
                    {
                        fixedTarget -= step;
                        break;
                    }

                    closesetDistance = dist;
                }
                //Debug.Log($"Fixed target {targetPosition} => {fp} => {fixedTarget}");
                targetPosition = fixedTarget;
            }

            return (path, targetPosition);
        }
        private static List<Point> CreatePath(Point startPoint, Point endPoint)
        {
            List<Point> openSet = new List<Point>();
            HashSet<Point> closeSet = new HashSet<Point>();

            startPoint.gCost = 0;
            startPoint.hCost = 0;
            startPoint.lastPoint = null;
            openSet.Add(startPoint);

            bool found = false;

            while (openSet.Count > 0)
            {
                Point currPoint = openSet[0];
                openSet.RemoveAt(0);

                closeSet.Add(currPoint);

                //Debug.Log($"Pathfinfing checking {currPoint.Name} (target: {endPoint.Name})");
                if (currPoint == endPoint)
                {
                    found = true;
                    break;
                }

                foreach (Point neighbour in currPoint.ConnectedPoints)
                {
                    if (closeSet.Contains(neighbour) || openSet.Contains(neighbour))
                        continue;

                    // set consts
                    neighbour.gCost = currPoint.gCost + currPoint.Distance(neighbour);
                    neighbour.hCost = Vector3.Distance(neighbour.transform.position, endPoint.transform.position);
                    neighbour.fCost = neighbour.hCost * distanceToEndMtltiplier + neighbour.gCost;
                    neighbour.lastPoint = currPoint;

                    // add to open set
                    int inserIndex = 0;
                    for (int i = openSet.Count - 1; i >= 0; i--)
                    {
                        if (neighbour.fCost >= openSet[i].fCost)
                        {
                            inserIndex = i + 1;
                            break;
                        }
                    }
                    openSet.Insert(inserIndex, neighbour);
                }
            }

            if (found)
            {
                // recrate path
                List<Point> points = new List<Point>() { startPoint };
                Point point = endPoint;
                while (startPoint != point)
                {
                    if (points == null || points.Contains(point))
                    {
                        Debug.LogWarning("Path is invalid");
                        return null;
                    }

                    points.Insert(1, point);
                    point = point.lastPoint;
                }
                return points;
            }
            else
            {
                Debug.LogWarning($"Dont found valid path from {startPoint.Name} to {endPoint.Name}");
                return null;
            }
        }


        private static Point FindClosestPoint(Vector3 position)
        {
            if (allPoints.Length == 0)
                return null;

            Point nearestPoint = allPoints[0];
            float minDist = Dist(nearestPoint);
            for (int i = 1; i < allPoints.Length; i++)
            {
                float dist = Dist(allPoints[i]);
                if (dist < minDist)
                {
                    nearestPoint = allPoints[i];
                    minDist = dist;
                }
            }
            return nearestPoint;

            float Dist(Point point) => Vector3.Distance(point.transform.position, position);
        }
        private static (Point closestPoint, Point secondLinePoint) FindClosestLine(Vector3 position)
        {
            Point firstLinePoint = null;
            Point secondLinePoint = null;
            float shortestDistance = float.MaxValue;
            float maxCheckedDistance = Instance.maxNeighbourDistance;
            foreach (Point firstpoint in allPoints)
            {
                float distanceToFPoint = Vector3.Distance(position, firstpoint.Position);
                if (distanceToFPoint > maxCheckedDistance)
                    continue;

                foreach (Point secondPoint in firstpoint.ConnectedPoints)
                {
                    if (firstpoint is PointWithPortal pointWithPortal && secondPoint == pointWithPortal.ConnectedPortalPoint)
                    {
                        if (distanceToFPoint < shortestDistance)
                        {
                            firstLinePoint = firstpoint;
                            secondLinePoint = null;
                        }
                        continue;
                    }

                    float distanceToLine = DistanceToLine(position, firstpoint.Position, secondPoint.Position);
                    if (distanceToLine < shortestDistance)
                    {
                        shortestDistance = distanceToLine;
                        firstLinePoint = firstpoint;
                        secondLinePoint = secondPoint;
                    }
                }
            }

            if (firstLinePoint == null)
                return (FindClosestPoint(position), null);

            if (secondLinePoint == null)
                return (firstLinePoint, null);

            if (Vector3.Distance(position, firstLinePoint.Position) < Vector3.Distance(position, secondLinePoint.Position))
                return (firstLinePoint, secondLinePoint);
            else
                return (secondLinePoint, firstLinePoint);
        }


        private static float DistanceToLine(Vector3 point, Vector3 lineA, Vector3 lineB)
        {
            Vector3 ab = lineB - lineA;
            Vector3 av = point - lineA;

            if (Vector3.Dot(av, ab) <= 0f)
                return av.magnitude;

            Vector3 bv = point - lineB;

            if (Vector3.Dot(bv, ab) >= 0f)
                return bv.magnitude;

            return (Vector3.Cross(ab, av)).magnitude / (ab).magnitude;
        }
    }
}
