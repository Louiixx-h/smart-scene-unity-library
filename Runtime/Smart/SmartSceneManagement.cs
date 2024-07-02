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
        public IList<string> SceneGroup { get; private set; } = new List<string>();
        public IList<string> PersistentScenes { get; } = new List<string>();
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public int SceneCount => SceneManager.sceneCount;
        public Scene GetSceneAt(int index) => SceneManager.GetSceneAt(index);
        
        public void SwitchSceneGroupAsync(SceneConfig sceneConfig)
        {
            StartCoroutine(SwitchSceneGroupCoroutine(sceneConfig));
        }
        
        public void LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig)
        {
            if (sceneConfig.IgnoreIfAlreadyLoaded && sceneConfig.Scenes.All(scene => SceneGroup.Contains(scene))) return;
            StartCoroutine(LoadSceneToCurrentGroupCoroutine(sceneConfig));
        }

        public void LoadPersistentSceneAsync(SceneConfig sceneConfig)
        {
            if (sceneConfig.IgnoreIfAlreadyLoaded && sceneConfig.Scenes.All(scene => PersistentScenes.Contains(scene))) return;
            StartCoroutine(LoadPersistentSceneCoroutine(sceneConfig));
        }
        
        public void UnloadSceneAsync(string sceneName)
        {
            StartCoroutine(UnloadSceneCoroutine(sceneName));
        }

        public void UnloadPersistentSceneAsync(string sceneName)
        {
            StartCoroutine(UnloadPersistentSceneCoroutine(sceneName));
        }

        private IEnumerator SwitchSceneGroupCoroutine(SceneConfig sceneConfig)
        {
            OnLoadingStart?.Invoke();
            if (SceneGroup.Any())
            {
                yield return UnloadAllNonPersistentScenesCoroutine(SceneGroup);
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
            SceneGroup.Clear();
            SceneGroup = sceneConfig.Scenes;
            OnLoadingEnd?.Invoke();
        }

        private IEnumerator LoadSceneToCurrentGroupCoroutine(SceneConfig sceneConfig)
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
                SceneGroup.Add(scene);
            }
            OnLoadingEnd?.Invoke();
        }

        private IEnumerator LoadPersistentSceneCoroutine(SceneConfig sceneConfig)
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
                PersistentScenes.Add(scene);   
            }
            OnLoadingEnd?.Invoke();
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
            SceneGroup.Clear();
            Resources.UnloadUnusedAssets();
        }
        
        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            if (!SceneGroup.Contains(sceneName)) yield break;
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
            SceneGroup.Remove(sceneName);
            Resources.UnloadUnusedAssets();
        }

        private IEnumerator UnloadPersistentSceneCoroutine(string sceneName)
        {
            if (!PersistentScenes.Contains(sceneName)) yield break;
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
            PersistentScenes.Remove(sceneName);
            Resources.UnloadUnusedAssets();
        }
    }
}