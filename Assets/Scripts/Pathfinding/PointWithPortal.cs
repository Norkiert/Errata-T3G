using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Pathfinding
{
    public class PointWithPortal : Point
    {
        [SerializeField, Required] private PointWithPortal connectedPortalPoint;
        [SerializeField] private Transform portalOffset;

        [Button("Update Points")]
        private void ButtonUdatePoints() => Pathfinder.UpdatePoints();

        public PointWithPortal ConnectedPortalPoint => connectedPortalPoint;
        public Transform PortalOffset => portalOffset;
        public void SetConnectedPortalPoint(PointWithPortal point) => connectedPortalPoint = point;

        public override void FindConnectedPoints(Point[] allPoints)
        {
            base.FindConnectedPoints(allPoints);

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
            if (Pathfinder.ShowConnections)
            {
                Gizmos.color = Color.yellow;

                foreach (Point point in connectedPoints)
                    if (point && point != connectedPortalPoint)
                        Gizmos.DrawLine(Position, point.Position);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Position, 0.2f);

                if (connectedPortalPoint != null)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(Position, connectedPortalPoint.Position);
                }

                if (portalOffset != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(Position, portalOffset.position);
                }
            }
        }
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            foreach (Point point in connectedPoints)
                if (point && point != connectedPortalPoint)
                    Gizmos.DrawLine(Position, point.Position);

            Gizmos.DrawSphere(Position, 0.3f);

            if (connectedPortalPoint != null)
            {
                Gizmos.DrawSphere(connectedPortalPoint.Position, 0.6f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(Position, connectedPortalPoint.Position);
            }

            if (portalOffset != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(Position, portalOffset.position);
            }
        }
    }
}
