using DG.Tweening;
using NaughtyAttributes;
using Portals;
using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dialogues;

namespace GameManagment
{
    public class DimensionManager : MonoBehaviour
    {
        [Header("Portal")]
        [SerializeField, Required] private Portal mainHubPortal;

        [Header("Dimension changing")]
        [SerializeField, Required] private MeshRenderer mainPortalSwitchingPlane;
        [SerializeField, Min(0.01f)] private float mainPortalSwitchingTime = 1f;

        [Header("Pathfinding")]
        [SerializeField, Required] private PointWithPortal mainPortalPathindingPoint;

        [Header("Dimension")]
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

            // desactive current dimension
            if (LoadedDimension != null)
            {
                DesactiveDimension(LoadedDimension);
                yield return new WaitForSeconds(0.2f);
            }

            // close portal
            {
                Color shitchingColor = mainPortalSwitchingPlane.material.color;
                float switchPercent = shitchingColor.a;
                while (switchPercent < 1)
                {
                    switchPercent += Time.deltaTime / mainPortalSwitchingTime;
                    shitchingColor.a = switchPercent;
                    mainPortalSwitchingPlane.material.color = shitchingColor;
                    yield return null;
                }
            }

            // unload current dimension
            if (LoadedDimension != null)
            {
                SaveManager.SaveGame();

                Debug.Log($"Unloading {LoadedDimension}");
                AsyncOperation unloadingDimension = SceneManager.UnloadSceneAsync(LoadedDimension.SceneName);

                while (!unloadingDimension.isDone)
                    yield return null;

                Debug.Log($"Unloaded {LoadedDimension}");
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

                SaveManager.LoadGame();

                LoadedDimension = dimensionToLoad;
                Debug.Log($"Loaded {LoadedDimension}");
            }

            yield return null;
            dimenionChanger = null;

            // fix if changed dimensionToLoad 
            if (LoadedDimension != dimensionToLoad)
            {
                LoadDimension(dimensionToLoad);
                yield break;
            }


            // active new dimension
            ActiveDimension(LoadedDimension);
            yield return new WaitForSeconds(0.2f);

            UpdateCamera();


            // open portal
            {
                Color shitchingColor = mainPortalSwitchingPlane.material.color;
                float switchPercent = shitchingColor.a;
                while (switchPercent > 0)
                {
                    switchPercent -= Time.deltaTime / mainPortalSwitchingTime;
                    shitchingColor.a = switchPercent;
                    mainPortalSwitchingPlane.material.color = shitchingColor;
                    yield return null;
                }
            }
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

            if (dimension.BasicDialogues != null)
                DialogueInkKeeper.AddNewText(dimension.BasicDialogues);

            // active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(dimension.SceneName));

            // set
            DimensionCore dimensionCore = GetDimensionCore(dimension);
            if (dimensionCore == null)
            {
                Debug.LogError("DimensionCore not found!");
                return;
            }

            if (dimensionCore.MainPortal == null)
                Debug.LogError("Dont found dimension main portal!");
            else
            {
                // link portals
                dimensionCore.MainPortal.SetLinkedPortal(mainHubPortal);
                mainHubPortal.SetLinkedPortal(dimensionCore.MainPortal);

                // set cameras background
                PlayerController playerController = FindObjectOfType<PlayerController>();
                DimensionSO dimensionSO = dimensionCore.DimensionSO;
                switch (dimensionCore.DimensionSO.CameraBackground)
                {
                    case DimensionSO.CameraBackgroundType.SkyBox:
                        dimensionCore.MainPortal.SetCameraBackgroundOnSkyBox();
                        playerController?.SetCameraBackgroundOnSkyBox(dimensionSO.CameraViewRange);
                        break;
                    case DimensionSO.CameraBackgroundType.SolidColor:
                        dimensionCore.MainPortal.SetCameraBackgroundOnSolidColor(dimensionSO.CameraBackgroundColor);
                        playerController?.SetCameraBackgroundOnSolidColor(dimensionSO.CameraBackgroundColor, dimensionSO.CameraViewRange);
                        break;
                }
            }

            if (dimensionCore.MainPathfindingPortalPoint == null)
                Debug.LogWarning("Dont found dimension main pathfinding portal point!");
            else
            {
                dimensionCore.MainPathfindingPortalPoint.SetConnectedPortalPoint(mainPortalPathindingPoint);
                mainPortalPathindingPoint.SetConnectedPortalPoint(dimensionCore.MainPathfindingPortalPoint);
            }
        }
        private void DesactiveDimension(DimensionSO dimension)
        {
            Debug.Log($"Desactive {dimension}");

            mainHubPortal.SetLinkedPortal(null);

            mainPortalPathindingPoint.SetConnectedPortalPoint(null);

            if (dimension.BasicDialogues != null)
                DialogueInkKeeper.RemoveText(dimension.BasicDialogues);
        }

        private DimensionCore GetDimensionCore(DimensionSO dimension)
        {
            foreach (DimensionCore dimCore in FindObjectsOfType<DimensionCore>())
                if (dimCore.DimensionSO == dimension)
                    return dimCore;

            return null;
        }
    }
}