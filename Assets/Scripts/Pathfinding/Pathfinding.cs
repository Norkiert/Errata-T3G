using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public float neighboursDistance = 30f;

    [SerializeField] Point start;
    [SerializeField] Point end;

    private Point[] allPoints;
    private CubeController cube;

    private List<Point> openList = new List<Point>();
    private List<Point> closedList = new List<Point>();
    private List<Point> Path = new List<Point>();

    private struct pointWithCost
    {
        public Point obj;
        public float fCost;
    }
    void Start()
    {
        cube = FindObjectOfType<CubeController>();
        allPoints = FindObjectsOfType<Point>();
        FindPathToPoint(start, end);
        Debug.Log(Path);
    }
    public void FindStartAndEndPoints(Vector3 target)
    {
        Point startPoint = FindClosestPoint(transform.position);
        Point endPoint = FindClosestPoint(target);
        FindPathToPoint(startPoint, endPoint);
        cube.GoToPoint(startPoint.transform.position);
    }

    private Point FindClosestPoint(Vector3 position)
    {
        float minDist = 10000f;
        if (allPoints.Length != 0)
        {
            Point nearestPoint = allPoints[0];
            foreach (Point point in allPoints)
            {
                if (Vector3.Distance(point.transform.position, position) < minDist)
                {
                    nearestPoint = point;
                    minDist = Vector3.Distance(point.transform.position, transform.position);
                }
            }
            return nearestPoint;
        }
        return null;

    }
    public void FindPathToPoint(Point startPoint, Point endPoint)
    {
        startPoint.gCost = 0;
        closedList.Add(startPoint);
        FindNextPoint(startPoint, endPoint);
    }
    private void FindNextPoint(Point point, Point endPoint)
    {
        foreach (Point node in point.neighbours)
        {
            if (node.transform.position == endPoint.transform.position)
            {
                Path.Add(endPoint);
                return;
            }
            node.gCost = Vector3.Distance(point.transform.position,node.transform.position);
            node.hCost = Vector3.Distance(node.transform.position, endPoint.transform.position);
            node.fCost = node.hCost + node.gCost;
        }
        pointWithCost temp = new pointWithCost();
        temp.obj = point;
        temp.fCost = 2000f;
        foreach(Point node in point.neighbours)
        {
            if(node.fCost<temp.fCost)
            {
                temp.fCost = node.fCost;
                temp.obj = node;
            }
        }
        temp.obj.lastPoint = point;
        closedList.Add(temp.obj);
        Path.Add(temp.obj);
        FindNextPoint(temp.obj,endPoint);
    }
}
    
