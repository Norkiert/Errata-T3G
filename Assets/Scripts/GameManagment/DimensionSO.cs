using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace GameManagment
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DimensionSO")]
    public class DimensionSO : ScriptableObject
    {
        [field: SerializeField] public string DimensionName { get; private set; } = "";
        [field: SerializeField] public string SceneName { get; private set; } = "";
    }
}