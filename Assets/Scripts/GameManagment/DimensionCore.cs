using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Portals;

namespace GameManagment
{
    public class DimensionCore : MonoBehaviour
    {
        [field: SerializeField, ValidateInput(nameof(IsValidThisDimension), "Dimenion can't be None")]
        public DimensionManager.Dimension ThisDimension { get; private set; } = DimensionManager.Dimension.None;


        [field: SerializeField, Required]
        public Portal MainPortal { get; private set; }


        private bool IsValidThisDimension() => ThisDimension != DimensionManager.Dimension.None;
    }
}
