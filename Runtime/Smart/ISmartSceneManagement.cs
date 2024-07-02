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
        public IList<string> SceneGroup { get; }
        public IList<string> PersistentScenes { get; }
        public Scene ActiveScene { get; }
        public int SceneCount { get; }
        public Scene GetSceneAt(int index);
        public void SwitchSceneGroupAsync(SceneConfig sceneConfig);
        public void LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig);
        public void LoadPersistentSceneAsync(SceneConfig sceneConfig);
        public void UnloadSceneAsync(string sceneName);
        public void UnloadPersistentSceneAsync(string sceneName);
    }
}