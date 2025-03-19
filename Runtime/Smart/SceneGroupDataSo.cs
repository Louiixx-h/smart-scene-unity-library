using System.Collections.Generic;
using UnityEngine;

namespace Com.LuisLabs.SmartScene
{
    [CreateAssetMenu(fileName = "NewSceneGroupData", menuName = "SmartScene/SceneGroupDataSo", order = 51)]
    public class SceneGroupDataSo : ScriptableObject
    {
        [SerializeField] private string groupName;
        [SerializeField] public List<string> scenes;
        
        /// <summary>
        /// Retorna o struct SceneGroupData armazenado neste ScriptableObject.
        /// </summary>
        /// <returns>O struct SceneGroupData.</returns>
        public SceneGroupData GetData()
        {
            return new SceneGroupData
            {
                name = groupName,
                scenes = scenes
            };
        }
    }
}