using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathfindingScripts;
public class Point : MonoBehaviour
{
    [HideInInspector] public float fCost; // summary cost
    [HideInInspector] public float gCost; // distance from start
    [HideInInspector] public float hCost; // distance from end

    [HideInInspector] public Point lastPoint;
    public readonly List<Point> neighbours = new List<Point>();

    public void FindNeighbours(List<Point> allPoints)
    {
        neighbours.Clear();
        foreach (Point point in allPoints)
        {
            if (point == this)
                continue;

            float dist = Vector3.Distance(transform.position, point.transform.position);
            if (dist <= Pathfinding.MaxNeighbourDistance)
            {
                Ray ray = new Ray(transform.position, (point.transform.position - transform.position));
                Physics.Raycast(ray, out RaycastHit hit, dist);

                if (hit.collider == null)
                    neighbours.Add(point);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (Pathfinding.ShowConnections)
        {
            Gizmos.color = Color.yellow;
            foreach (Point point in neighbours)
                Gizmos.DrawLine(transform.position, point.transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Point point in neighbours)
            Gizmos.DrawLine(transform.position, point.transform.position);
    }
}
