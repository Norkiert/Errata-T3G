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


        [SerializeField] private bool hasCustomMaxNeighbourDistance = false;
        [SerializeField, Min(0), ShowIf(nameof(hasCustomMaxNeighbourDistance))] float customMaxNeighbourDistance = 10f;


        [SerializeField, ReadOnly] protected List<Point> connectedPoints = new List<Point>();
        public List<Point> ConnectedPoints => connectedPoints;
        public virtual void FindNeighbours(Point[] allPoints)
        {
            if (connectedPoints != null)
                connectedPoints.Clear();
            else
                connectedPoints = new List<Point>();

            float maxDistance = hasCustomMaxNeighbourDistance ? customMaxNeighbourDistance : Pathfinding.MaxNeighbourDistance;
            LayerMask pathBlockers = Pathfinding.PathBlockers;

            foreach (Point point in allPoints)
            {
                if (point == this)
                    continue;

                float dist = Vector3.Distance(transform.position, point.transform.position);
                if (dist <= maxDistance)
                {
                    Ray ray = new Ray(transform.position, (point.transform.position - transform.position));
                    Physics.Raycast(ray, out RaycastHit hit, dist - 0.5f, pathBlockers);

                    if (hit.collider == null)
                        connectedPoints.Add(point);
                }
            }
        }


        [Button("Update Points")]
        private void ButtonUdatePoints() => Pathfinding.UpdatePoints();

        public virtual float Distance(Point secondPoint) => Vector3.Distance(Position, secondPoint.Position);

        public Vector3 Position => transform.position;
        public string Name => $"{name}({transform.parent.name})";


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

