using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Portals;
using Pathfinding;

namespace GameManagment
{
    public class DimensionCore : MonoBehaviour
    {
        [field: SerializeField, Required]
        public DimensionSO DimensionSO { get; private set; } = null;


        [field: SerializeField, Required]
        public Portal MainPortal { get; private set; } = null;


        [field: SerializeField, Required]
        public PointWithPortal MainPathfindingPortalPoint { get; private set; } = null;
    }
}
