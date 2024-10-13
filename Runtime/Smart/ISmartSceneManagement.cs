using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuisLabs.SmartScene
{
    public interface ISmartSceneManagement
    {
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public IList<string> CurrentSceneGroup { get; }
        public IList<string> CurrentPersistentScenes { get; }
        public Scene ActiveScene { get; }
        public int SceneCount { get; }
        public Scene GetSceneAt(int index);
        public IEnumerator SwitchSceneGroupAsync(SceneConfig sceneConfig);
        public IEnumerator LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig);
        public IEnumerator LoadPersistentSceneAsync(SceneConfig sceneConfig);
        public IEnumerator UnloadSceneAsync(string sceneName);
        public IEnumerator UnloadPersistentSceneAsync(string sceneName);
    }
}