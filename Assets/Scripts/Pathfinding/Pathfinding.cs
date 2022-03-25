using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PathFinding
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField, Min(1)] private float maxNeighbourDistance = 60f;
        [SerializeField] private bool showConnections = false;

        private Point[] allPoints;


        [Button("UpdatePoints")]
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
        public static bool ShowConnections => Instance == null ? false : Instance.showConnections;

        private void Start()
        {
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            allPoints = FindObjectsOfType<Point>();
            foreach (Point point in allPoints)
                point.FindNeighbours(allPoints);
        }
        public static (List<Point> path, Vector3 fixedTarget) FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            Instance.UpdatePoints();
            if (Instance.allPoints.Length > 1)
            {
                Point start = Instance.FindClosestPoint(startPosition);
                Point end = Instance.FindClosestPoint(targetPosition);

                // try find better end point
                if (start == end)
                {
                    float shortestDist = float.MaxValue;
                    Point bestPoint = null;
                    foreach (Point secondPoint in end.neighbours)
                    {
                        float dist = DistanceToShiftedPoint(secondPoint);
                        if (dist < shortestDist)
                        {
                            shortestDist = dist;
                            bestPoint = secondPoint;
                        }
                    }

                    if (DistanceToShiftedPoint(bestPoint) < Vector3.Distance(end.transform.position, targetPosition))
                        end = bestPoint;

                    float DistanceToShiftedPoint(Point secondPoint) => Vector3.Distance(end.transform.position + (secondPoint.transform.position - end.transform.position).normalized, targetPosition);
                }

                List<Point> path = FindFinalPath(start, end);

                // fix target position
                if (path.Count > 1)
                {
                    Vector3 fp = path[path.Count - 2].Position;
                    Vector3 sp = path[path.Count - 1].Position;
                    Vector3 dir = (fp - sp).normalized;
                    Vector3 fixedTarget = sp;
                    float closesetDistance = Vector3.Distance(fixedTarget, targetPosition); 
                    while (true)
                    {
                        fixedTarget += dir;
                        float dist = Vector3.Distance(fixedTarget, targetPosition);
                        if (dist > closesetDistance)
                        {
                            fixedTarget -= dir;
                            break;
                        }

                        closesetDistance = dist;
                    }
                    //Debug.Log($"Fixed target {targetPosition} => {fixedTarget}");
                    targetPosition = fixedTarget;
                }

                return (path, targetPosition);
            }
            else
                return (null, targetPosition);
            
        }
        private Point FindClosestPoint(Vector3 position)
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


        private static List<Point> FindFinalPath(Point startPoint, Point endPoint)
        {
            if (CreatePath(startPoint, endPoint) == false)
            {
                Debug.LogWarning("Dont found valid path");
                return null;
            }

            return RecreatePath(startPoint, endPoint);
        }
        private static bool CreatePath(Point startPoint, Point endPoint)
        {
            List<Point> openSet = new List<Point>();
            HashSet<Point> closeSet = new HashSet<Point>();

            startPoint.gCost = 0;
            startPoint.hCost = 0;
            startPoint.lastPoint = null;
            openSet.Add(startPoint);

            while (openSet.Count > 0)
            {
                Point currPoint = openSet[0];
                openSet.RemoveAt(0);

                closeSet.Add(currPoint);

                if (currPoint == endPoint)
                    return true;


                foreach (Point neighbour in currPoint.neighbours)
                {
                    if (closeSet.Contains(neighbour) || openSet.Contains(neighbour))
                        continue;

                    // set consts
                    neighbour.gCost = currPoint.gCost + Vector3.Distance(currPoint.transform.position, neighbour.transform.position);
                    neighbour.hCost = Vector3.Distance(neighbour.transform.position, endPoint.transform.position);
                    neighbour.fCost = neighbour.hCost + neighbour.gCost;
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

            return false;
        }
        private static List<Point> RecreatePath(Point startPoint, Point endPoint)
        {
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
    }
}
