using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.LuisLabs.SmartScene
{
    public class SmartSceneManagement : MonoBehaviour, ISmartSceneManagement
    {
        public SceneGroupData CurrentSceneGroup { get; private set; }
        public SceneGroupData CurrentPersistentSceneGroup { get; private set; }
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public int SceneCount => SceneManager.sceneCount;

        public Scene GetSceneAt(int index) => SceneManager.GetSceneAt(index);

        public IEnumerator SwitchSceneGroupAsync(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            yield return UnloadAllNonPersistentScenesCoroutine(CurrentSceneGroup.scenes);
            yield return LoadScenesCoroutine(sceneConfig);
            CurrentSceneGroup = sceneConfig.SceneGroup;
            OnLoadingEnd?.Invoke();
        }

        public IEnumerator LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            yield return LoadScenesCoroutine(sceneConfig);
            CurrentSceneGroup.AddAllScenes(sceneConfig.SceneGroup.scenes);
            OnLoadingEnd?.Invoke();
        }

        public IEnumerator LoadPersistentSceneAsync(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            yield return LoadScenesCoroutine(sceneConfig);
            CurrentPersistentSceneGroup.AddAllScenes(sceneConfig.SceneGroup.scenes);
            OnLoadingEnd?.Invoke();
        }

        public IEnumerator UnloadSceneAsync(string sceneName)
        {
            if (!CurrentSceneGroup.ContainsScene(sceneName)) yield break;
            yield return UnloadSceneCoroutine(sceneName);
            CurrentSceneGroup.RemoveScene(sceneName);
        }

        public IEnumerator UnloadPersistentSceneAsync(string sceneName)
        {
            if (!CurrentPersistentSceneGroup.ContainsScene(sceneName)) yield break;
            yield return UnloadSceneCoroutine(sceneName);
            CurrentPersistentSceneGroup.RemoveScene(sceneName);
        }

        private IEnumerator LoadScenesCoroutine(SceneConfig sceneConfig)
        {
#if UNITY_EDITOR
            print("Loading scene...");
#endif
            var operations = sceneConfig
                .SceneGroup
                .scenes
                .Select(scene => SceneManager.LoadSceneAsync(
                    scene,
                    LoadSceneMode.Additive)
                ).ToList();

            if (operations.Count == 0) yield break;

            foreach (var asyncOperation in operations)
            {
                asyncOperation.allowSceneActivation = false;
            }

            while (!operations.All(asyncOperation => asyncOperation.isDone))
            {
                sceneConfig.Progress?.Report(operations.Average(asyncOperation => asyncOperation.progress));
#if UNITY_EDITOR
                Debug.Log($"Loading progress: {operations.Average(asyncOperation => asyncOperation.progress) * 100}%");
#endif
                if (operations.Average(asyncOperation => asyncOperation.progress) >= 0.9f)
                {
                    sceneConfig.Progress?.Report(1);
                    foreach (var asyncOperation in operations)
                    {
                        asyncOperation.allowSceneActivation = true;
                    }

                    break;
                }

                yield return null;
            }
#if UNITY_EDITOR
            Debug.Log("Scene loaded!");
#endif
        }

        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (asyncOperation == null) yield break;
            while (!asyncOperation.isDone)
            {
#if UNITY_EDITOR
                Debug.Log($"Unloading progress: {asyncOperation.progress * 100}%");
#endif
                yield return null;
            }
#if UNITY_EDITOR
            Debug.Log("Scene unloaded!");
#endif
            Resources.UnloadUnusedAssets();
        }

        private IEnumerator UnloadAllNonPersistentScenesCoroutine(IList<string> scenes)
        {
            if (scenes == null || scenes.Count == 0) yield break;
            var operations = scenes.Select(SceneManager.UnloadSceneAsync).ToList();
            while (!operations.All(asyncOperation => asyncOperation.isDone))
            {
#if UNITY_EDITOR
                Debug.Log(
                    $"Unloading progress: {operations.Average(asyncOperation => asyncOperation.progress) * 100}%");
#endif
                yield return null;
            }
#if UNITY_EDITOR
            Debug.Log("Scene unloaded!");
#endif
            Resources.UnloadUnusedAssets();
        }
    }
}