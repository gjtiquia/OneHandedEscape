using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class LoadSceneManager : MonoBehaviour
    {
        private static LoadSceneManager _instance;

        [SerializeField] private bool _debug;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }

        // PUBLIC METHODS
        public static void LoadScene(string sceneName)
        {
            _instance.StartToLoadScene(sceneName);
        }

        // PRIVATE METHODS
        private void StartToLoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            string currentActiveSceneName = SceneManager.GetActiveScene().name;

            if (_debug)
                Debug.Log($"Current active scene: {currentActiveSceneName}");

            yield return LoadActiveAdditiveSceneCoroutine(SceneName.LoadingScene);
            yield return UnloadSceneCoroutine(currentActiveSceneName);
            yield return LoadActiveAdditiveSceneCoroutine(sceneName);
            yield return UnloadSceneCoroutine(SceneName.LoadingScene);
        }

        private IEnumerator LoadActiveAdditiveSceneCoroutine(string sceneName)
        {
            if (_debug)
                Debug.Log($"Loading the {sceneName}...");

            AsyncOperation loadGameSceneTask = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!loadGameSceneTask.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            if (_debug)
                Debug.Log($"{sceneName} loaded and set as active scene!");
        }

        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            if (_debug)
                Debug.Log($"Unloading {sceneName}...");

            AsyncOperation unloadActiveSceneTask = SceneManager.UnloadSceneAsync(sceneName);
            while (!unloadActiveSceneTask.isDone)
            {
                yield return null;
            }

            if (_debug)
                Debug.Log($"Unloaded {sceneName}!");
        }
    }
}