using System;

namespace LuisLabs.SmartScene
{
    public interface ISmartScene
    {
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public void AddScene(string scene);
        public void AddCurrentScene(string name);
        public void SwitchScene(string scene);
        public void SwitchSceneAsync(string scene);
    }
}