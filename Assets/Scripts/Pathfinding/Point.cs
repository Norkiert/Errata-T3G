using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PathFinding
{
    public class Point : MonoBehaviour
    {
        [HideInInspector] public Point lastPoint;
        [HideInInspector] public float fCost; // summary cost
        [HideInInspector] public float gCost; // distance to start
        [HideInInspector] public float hCost; // distance to end


        [SerializeField, ReadOnly] protected List<Point> connectedPoints = new List<Point>();
        public List<Point> ConnectedPoints => connectedPoints;
        public virtual void FindNeighbours(Point[] allPoints)
        {
            if (connectedPoints != null)
                connectedPoints.Clear();
            else
                connectedPoints = new List<Point>();

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
                        connectedPoints.Add(point);
                }
            }
        }

        public virtual float Distance(Point secondPoint) => Vector3.Distance(Position, secondPoint.Position);

        public Vector3 Position => transform.position;


        protected virtual void OnDrawGizmos()
        {
            if (Pathfinding.ShowConnections)
            {
                Gizmos.color = Color.yellow;

                foreach (Point point in connectedPoints)
                    if (point)
                        Gizmos.DrawLine(Position, point.Position);

                Gizmos.DrawSphere(Position, 0.3f);
            }
        }
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            foreach (Point point in connectedPoints)
                if (point)
                    Gizmos.DrawLine(Position, point.Position);

            Gizmos.DrawSphere(Position, 0.4f);
        }
    }
}

