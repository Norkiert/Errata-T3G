using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using Portals;
using DG.Tweening;

namespace GameManagment
{
    public class DimensionManager : MonoBehaviour
    {
        [SerializeField, Required] private Portal mainHubPortal;
        [SerializeField, Required] private DimensionSO defaultDimension;

        private DimensionSO dimensionToLoad;
        private IEnumerator dimenionChanger;


        private static DimensionManager instance;
        public static DimensionSO LoadedDimension { get; private set; } = null;
        public static DimensionSO DefaultDimension => instance.defaultDimension;

        private void Awake()
        {
            if (instance != null)
            {
                return;
            }

            instance = this;

            DOTween.SetTweensCapacity(200, 1100);
        }

        private void Start()
        {
            LoadDimension(defaultDimension);
        }

        public static void LoadDimension(DimensionSO dimension)
        {
            instance.dimensionToLoad = dimension;

            if (instance.dimenionChanger == null)
            {
                instance.dimenionChanger = instance.ChangeDimension();
                instance.StartCoroutine(instance.dimenionChanger);
            }  
        }

        private IEnumerator ChangeDimension()
        {
            if (dimensionToLoad == LoadedDimension)
            {
                dimenionChanger = null;
                Debug.LogWarning("Atempted to load already loaded dimension!");
                yield break;
            }

            // unload current dimension
            if (LoadedDimension != null)
            {
                DesactiveDimension(LoadedDimension);
                yield return null;

                Debug.Log($"Unloading {dimensionToLoad}");
                AsyncOperation unloadingDimension = SceneManager.UnloadSceneAsync(LoadedDimension.SceneName);

                while (!unloadingDimension.isDone)
                    yield return null;

                Debug.Log($"Unloaded {dimensionToLoad}");
                LoadedDimension = null;
                UpdateCamera();
            }

            // load new
            if (dimensionToLoad != null)
            {
                Debug.Log($"Loading {dimensionToLoad}");
                AsyncOperation loadingDimension = SceneManager.LoadSceneAsync(dimensionToLoad.SceneName, LoadSceneMode.Additive);

                while (!loadingDimension.isDone)
                    yield return null;

                LoadedDimension = dimensionToLoad;
                Debug.Log($"Loaded {LoadedDimension}");
            }

            yield return null;
            dimenionChanger = null;

            // fix if changed dimensionToLoad 
            if (LoadedDimension != dimensionToLoad)
                LoadDimension(dimensionToLoad);
            else
                ActiveDimension(LoadedDimension);

            yield return null;
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            foreach (MainCamera camera in FindObjectsOfType<MainCamera>())
            {
                camera.UpdatePortals();
            }
        }

        private void ActiveDimension(DimensionSO dimension)
        {
            Debug.Log($"Active {dimension}");

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(dimension.SceneName));

            DimensionCore dimensionCore = GetDimensionCore(dimension);
            if (dimensionCore == null)
            {
                Debug.LogError("DimensionCore not found!");
                return;
            }

            if (dimensionCore.MainPortal == null)
                Debug.LogError("Dimension main portal not found!");
            else
            {
                // link portals
                dimensionCore.MainPortal.SetLinkedPortal(mainHubPortal);
                mainHubPortal.SetLinkedPortal(dimensionCore.MainPortal);

                // set cameras background
                PlayerController playerController = FindObjectOfType<PlayerController>();
                DimensionSO dimensionSO = dimensionCore.ThisDimension;
                switch (dimensionCore.ThisDimension.CameraBackground)
                {
                    case DimensionSO.CameraBackgroundType.SkyBox:
                        dimensionCore.MainPortal.SetCameraBackgroundOnSkyBox();
                        playerController?.SetCameraBackgroundOnSkyBox();
                        break;
                    case DimensionSO.CameraBackgroundType.SolidColor:
                        dimensionCore.MainPortal.SetCameraBackgroundOnSolidColor(dimensionSO.CameraBackgroundColor);
                        playerController?.SetCameraBackgroundOnSolidColor(dimensionSO.CameraBackgroundColor);
                        break;
                }
            }
        }

        private void DesactiveDimension(DimensionSO dimension)
        {
            Debug.Log($"Desactive {dimension}");

            mainHubPortal.SetLinkedPortal(null);
        }

        private DimensionCore GetDimensionCore(DimensionSO dimension)
        {
            foreach (DimensionCore dimCore in FindObjectsOfType<DimensionCore>())
                if (dimCore.ThisDimension == dimension)
                    return dimCore;

            return null;
        }
    }
}