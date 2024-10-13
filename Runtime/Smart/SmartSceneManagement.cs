using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuisLabs.SmartScene
{
    public class SmartSceneManagement : MonoBehaviour, ISmartSceneManagement
    {
        public IList<string> CurrentSceneGroup { get; private set; } = new List<string>();
        public IList<string> CurrentPersistentScenes { get; } = new List<string>();
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public int SceneCount => SceneManager.sceneCount;
        public Scene GetSceneAt(int index) => SceneManager.GetSceneAt(index);
        
        public IEnumerator SwitchSceneGroupAsync(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            if (CurrentSceneGroup.Any())
            {
                yield return UnloadAllNonPersistentScenesCoroutine(CurrentSceneGroup);
            }

            var operations = sceneConfig.Scenes.Select(scene => SceneManager.LoadSceneAsync(
                scene,
                sceneConfig.LoadSceneMode)
            ).ToList();
            
            if (operations.Count == 0)
            {
                OnLoadingEnd?.Invoke();
                yield break;
            }
            
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
            CurrentSceneGroup.Clear();
            CurrentSceneGroup = sceneConfig.Scenes;
            OnLoadingEnd?.Invoke();
        }
        
        public IEnumerator LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            var operations = sceneConfig.Scenes.Select(scene => SceneManager.LoadSceneAsync(
                scene,
                sceneConfig.LoadSceneMode)
            ).ToList();
            
            if (operations.Count == 0)
            {
                OnLoadingEnd?.Invoke();
                yield break;
            }
            
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
            foreach (var scene in sceneConfig.Scenes)
            {
                CurrentSceneGroup.Add(scene);
            }
            OnLoadingEnd?.Invoke();
        }

        public IEnumerator LoadPersistentSceneAsync(SceneConfig sceneConfig)
        {
            var operations = sceneConfig.Scenes.Select(scene => SceneManager.LoadSceneAsync(
                scene,
                sceneConfig.LoadSceneMode)
            ).ToList();
            if (operations.Count == 0) yield break;
            OnLoadingStart?.Invoke();
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
            foreach (var scene in sceneConfig.Scenes)
            {
                CurrentPersistentScenes.Add(scene);   
            }
            OnLoadingEnd?.Invoke();
        }
        
        public IEnumerator UnloadSceneAsync(string sceneName)
        {
            if (!CurrentSceneGroup.Contains(sceneName)) yield break;
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
            CurrentSceneGroup.Remove(sceneName);
            Resources.UnloadUnusedAssets();
        }

        public IEnumerator UnloadPersistentSceneAsync(string sceneName)
        {
            if (!CurrentPersistentScenes.Contains(sceneName)) yield break;
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
            CurrentPersistentScenes.Remove(sceneName);
            Resources.UnloadUnusedAssets();
        }

        private IEnumerator UnloadAllNonPersistentScenesCoroutine(IList<string> scenes)
        {
            var operations = scenes.Select(SceneManager.UnloadSceneAsync).ToList();
            while (!operations.All(asyncOperation => asyncOperation.isDone))
            {
#if UNITY_EDITOR
                Debug.Log($"Unloading progress: {operations.Average(asyncOperation => asyncOperation.progress) * 100}%");
#endif
                yield return null;
            }
#if UNITY_EDITOR
            Debug.Log("Scene unloaded!");
#endif
            CurrentSceneGroup.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}