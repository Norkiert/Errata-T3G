using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class Point : MonoBehaviour
    {
        [HideInInspector] public float fCost; // summary cost
        [HideInInspector] public float gCost; // distance from start
        [HideInInspector] public float hCost; // distance from end

        [HideInInspector] public Point lastPoint;
        public List<Point> neighbours = new List<Point>();

        public void FindNeighbours(Point[] allPoints)
        {
            if (neighbours != null)
                neighbours.Clear();
            else
                neighbours = new List<Point>();

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

        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            if (Pathfinding.ShowConnections)
            {
                Gizmos.color = Color.yellow;
                foreach (Point point in neighbours)
                    if (point)
                        Gizmos.DrawLine(transform.position, point.transform.position);

                Gizmos.DrawSphere(transform.position, 0.3f);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (Point point in neighbours)
                if (point)
                    Gizmos.DrawLine(transform.position, point.transform.position);

            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}

