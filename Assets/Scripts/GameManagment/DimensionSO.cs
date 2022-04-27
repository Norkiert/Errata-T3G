using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace GameManagment
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DimensionSO")]
    public class DimensionSO : ScriptableObject
    {
        public enum CameraBackgroundType { SkyBox, SolidColor }

        [field: Header("Names")]
        [field: SerializeField] public string DimensionName { get; private set; } = "";
        [field: SerializeField] public string SceneName { get; private set; } = "";


        [field: Header("Environment")]
        [field: SerializeField] public CameraBackgroundType CameraBackground { get; private set; } = CameraBackgroundType.SkyBox;
        [field: SerializeField, ShowIf(nameof(IsShownCameraBackgroundColor))] public Color CameraBackgroundColor { get; private set; } = Color.white;
        [field: SerializeField] public float CameraViewRange { get; private set; } = 1000f;

        [field: Header("Dialoguues")]
        [field: SerializeField] public TextAsset BasicDialogues { get; private set; } = null;


        #region -inspektor methods-

        private bool IsShownCameraBackgroundColor() => CameraBackground == CameraBackgroundType.SolidColor;

        #endregion
    }
}