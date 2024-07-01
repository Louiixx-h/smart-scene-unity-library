using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuisLabs.SmartScene
{
    public class SimpleSceneManagement : MonoBehaviour, ISimpleSceneManagement
    {
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }

        public void LoadSceneAsync(string scene, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            StartCoroutine(LoadSingleSceneOperationAsync(scene, mode));
        }

        public void SwitchSceneAsync(GameObject gameObj, string sceneName)
        {
            StartCoroutine(SwitchOperationAsync(gameObj, sceneName));
        }
        
        private IEnumerator LoadSingleSceneOperationAsync(string sceneName, LoadSceneMode mode)
        {
            yield return null;
            OnLoadingStart?.Invoke();
            yield return LoadSceneCoroutine(sceneName, mode);
            OnLoadingEnd?.Invoke();
            yield return null;
        }

        private IEnumerator SwitchOperationAsync(GameObject gameObj, string sceneName)
        {
            yield return null;
            OnLoadingStart?.Invoke();
            yield return UnloadSceneCoroutine(gameObj.scene.name);
            yield return LoadSceneCoroutine(sceneName);
            OnLoadingEnd?.Invoke();
            yield return null;
        }

        private IEnumerator UnloadSceneCoroutine(string scene)
        {
            yield return null;
            var asyncOperation = SceneManager.UnloadSceneAsync(scene);
            while (asyncOperation is { isDone: false })
            {
#if UNITY_EDITOR
                Debug.Log($"Unloading {scene} progress: {asyncOperation.progress * 100}%");
#endif
                yield return null;
            }
#if UNITY_EDITOR
            Debug.Log($"Scene {scene} unloaded!");
#endif
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            yield return null;
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (asyncOperation == null) yield break;
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
#if UNITY_EDITOR
                Debug.Log($"Loading {sceneName} progress: {asyncOperation.progress * 100}%");
#endif
                if (asyncOperation.progress >= 0.9f)
                {
#if UNITY_EDITOR
                    Debug.Log($"Scene {sceneName} loaded!");
#endif
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}