using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagment
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;

        public static GameManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            loadingScreen.SetActive(false);
        }

        public void LoadFirstGame()
        {
            if (loadFirstGameC != null)
                return;

            loadFirstGameC = LoadFirstGameC("Hub_Scene");
            StartCoroutine(loadFirstGameC);
        }

        private IEnumerator loadFirstGameC;
        private IEnumerator LoadFirstGameC(string sceneName)
        {
            loadingScreen.SetActive(true);

            Debug.Log($"Loading {sceneName}");
            AsyncOperation loadingDimension = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!loadingDimension.isDone)
                yield return null;

            Debug.Log($"Loaded {sceneName}");


            // wait for load main dimension
            while(DimensionManager.LoadedDimension != DimensionManager.Dimension.Main)
                yield return null;


            yield return new WaitForSeconds(0.1f);
            loadingScreen.SetActive(false);

            // set player position
            GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint");
            PlayerController player = FindObjectOfType<PlayerController>();
            if (spawnPoint == null)
                Debug.LogWarning("Dont found PlayerSpawnPoint");
            else if (player == null)
                Debug.LogWarning("Dont found Player");
            else
            {
                player.SetPosition(spawnPoint.transform.position);
                player.SetRotation(spawnPoint.transform.eulerAngles.x, spawnPoint.transform.eulerAngles.y);
            }

            loadFirstGameC = null;
        }
    }
}