using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaughtyAttributes;

namespace GameManagment
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Required] private GameObject loadingScreen;
        [SerializeField, Required] private Slider loadingBar;
        public static event Action OnPauseGame;
        public static event Action OnResumeGame;
        public static bool IsGamePaused { get; private set; } = false;

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
            loadingBar.value = 0;
            loadingScreen.SetActive(true);

            Debug.Log($"Loading {sceneName}");
            AsyncOperation loadingDimension = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!loadingDimension.isDone)
            {
                loadingBar.value = loadingDimension.progress / 0.9f * 0.5f;
                yield return null;
            }    

            Debug.Log($"Loaded {sceneName}");


            // wait for load main dimension
            while(DimensionManager.LoadedDimension != DimensionManager.DefaultDimension)
                yield return null;


            const float loadingTime = 0.7f;
            float waitTime = 0;
            while(waitTime <= loadingTime)
            {
                waitTime += Time.unscaledDeltaTime;
                loadingBar.value = waitTime / loadingTime * 0.5f + 0.5f;
                yield return null;
            }
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

        // Escape menu handling
        public static void PauseGame()
        {
            IsGamePaused = true;
            SetCursorState(false);
            OnPauseGame.Invoke();
        }
        public static void ResumeGame()
        {
            IsGamePaused = false;
            SetCursorState(true);
            OnResumeGame.Invoke();
        }
        public static void SetCursorState(bool isLock)
        {
            if (isLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
                Cursor.visible = false;
            }
            else
            {
                Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}