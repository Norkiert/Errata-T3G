using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PathFinding
{
    public class PointWithPortal : Point
    {
        [SerializeField, Required] private PointWithPortal connectedPortalPoint;

        [Button("Update Points")]
        private void ButtonUdatePoints() => Pathfinding.UpdatePoints();

        public PointWithPortal ConnectedPortalPoint => connectedPortalPoint;

        public override void FindNeighbours(Point[] allPoints)
        {
            base.FindNeighbours(allPoints);

            if (connectedPortalPoint != null &&!connectedPoints.Contains(connectedPortalPoint))
                connectedPoints.Add(connectedPortalPoint);
        }

        public override float Distance(Point secondPoint)
        {
            if (secondPoint == connectedPortalPoint)
                return 0;

            return base.Distance(secondPoint);
        }

        private void OnValidate()
        {
            if (connectedPortalPoint == this)
            {
                connectedPortalPoint = null;
                Debug.LogWarning($"{name}: cant set {nameof(connectedPortalPoint)} at itself!");
            }
        }

        protected override void OnDrawGizmos()
        {
            if (Pathfinding.ShowConnections)
            {
                Gizmos.color = Color.yellow;

                foreach (Point point in connectedPoints)
                    if (point && point != connectedPortalPoint)
                        Gizmos.DrawLine(Position, point.Position);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Position, 0.5f);

                if (connectedPortalPoint != null)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(Position, connectedPortalPoint.Position);
                } 
            }
        }
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            foreach (Point point in connectedPoints)
                if (point && point != connectedPortalPoint)
                    Gizmos.DrawLine(Position, point.Position);

            Gizmos.DrawSphere(Position, 0.6f);

            if (connectedPortalPoint != null)
            {
                Gizmos.DrawSphere(connectedPortalPoint.Position, 0.6f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(Position, connectedPortalPoint.Position);
            }
                
        }
    }
}
