using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuisLabs.SmartScene
{
    public interface ISimpleSceneManagement
    {
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public void LoadSceneAsync(string scene, LoadSceneMode mode = LoadSceneMode.Additive);
        public void SwitchSceneAsync(string currentScene, string sceneName);
    }
}