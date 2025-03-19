using System;
using System.Collections.Generic;

namespace Com.LuisLabs.SmartScene
{
    /// <summary>
    /// Data structure for scene groups.
    /// </summary>
    [Serializable]
    public struct SceneGroupData
    {
        public string name;
        public List<string> scenes;
        
        internal void AddScene(string sceneName)
        {
            scenes ??= new List<string>();
            scenes.Add(sceneName);
        }
        
        internal void AddAllScenes(List<string> sceneNames)
        {
            scenes ??= new List<string>();
            scenes.AddRange(sceneNames);
        }

        internal void RemoveScene(string sceneName)
        {
            scenes?.Remove(sceneName);
        }

        internal bool ContainsScene(string sceneName)
        {
            return scenes?.Contains(sceneName) ?? false;
        }
    }
}