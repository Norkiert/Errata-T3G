using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Portals;

namespace GameManagment
{
    public class DimensionCore : MonoBehaviour
    {
        [field: SerializeField, Required]
        public DimensionSO ThisDimension { get; private set; } = null;


        [field: SerializeField, Required]
        public Portal MainPortal { get; private set; } = null;
    }
}
