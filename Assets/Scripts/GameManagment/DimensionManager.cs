using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using Portals;

namespace GameManagment
{
    public class DimensionManager : MonoBehaviour
    {
        public enum Dimension { None, Main, SpaceLaser, Steampunk }


        [SerializeField, Required] private Portal mainHubPortal;


        private Dimension dimensionToLoad;
        private IEnumerator dimenionChanger;


        private static DimensionManager instance;
        public static Dimension LoadedDimension { get; private set; } = Dimension.None;


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
            LoadDimension(Dimension.Main);
        }

        public static void UnloadLoadedDimension() => LoadDimension(Dimension.Main);
        public static void LoadDimension(Dimension dimension)
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
            if (LoadedDimension != Dimension.None)
            {
                DesactiveDimension(LoadedDimension);
                yield return null;

                Debug.Log($"Unloading {dimensionToLoad}");
                AsyncOperation unloadingDimension = SceneManager.UnloadSceneAsync(DimensionName(LoadedDimension));

                while (!unloadingDimension.isDone)
                    yield return null;

                Debug.Log($"Unloaded {dimensionToLoad}");
                LoadedDimension = Dimension.None;
                FindObjectOfType<MainCamera>()?.GetPortals();
            }

            // load new
            if (dimensionToLoad != Dimension.None)
            {
                Debug.Log($"Loading {dimensionToLoad}");
                AsyncOperation loadingDimension = SceneManager.LoadSceneAsync(DimensionName(dimensionToLoad), LoadSceneMode.Additive);

                while (!loadingDimension.isDone)
                    yield return null;

                LoadedDimension = dimensionToLoad;
                Debug.Log($"Loaded {LoadedDimension}");
                FindObjectOfType<MainCamera>()?.GetPortals();
            }

            dimenionChanger = null;

            // fix if changed dimensionToLoad 
            if (LoadedDimension != dimensionToLoad)
                LoadDimension(dimensionToLoad);
            else
                ActiveDimension(LoadedDimension);
        }

        private string DimensionName(Dimension dimension)
        {
            return dimension switch
            {
                Dimension.Main => "MainDimension_Scene",
                Dimension.SpaceLaser => "SpaceLaser_Scene",
                Dimension.Steampunk => "Steampunk_Scene",
                _ => ""
            };
        }

        private void ActiveDimension(Dimension dimension)
        {
            Debug.Log($"Active {dimension}");

            // przeniesæ do przechodenia przez portale
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(DimensionName(dimension)));

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
                dimensionCore.MainPortal.SetLinkedPortal(mainHubPortal);
                mainHubPortal.SetLinkedPortal(dimensionCore.MainPortal);
            }
        }

        private void DesactiveDimension(Dimension dimension)
        {
            Debug.Log($"Desactive {dimension}");

            mainHubPortal.SetLinkedPortal(null);
        }

        private DimensionCore GetDimensionCore(Dimension dimension)
        {
            foreach (DimensionCore dimCore in FindObjectsOfType<DimensionCore>())
                if (dimCore.ThisDimension == Dimension.Main)
                    return dimCore;

            return null;
        }
    }
}