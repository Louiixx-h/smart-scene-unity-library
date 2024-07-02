using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace LuisLabs.SmartScene
{
    public class SceneConfig
    {
        public LoadSceneMode LoadSceneMode { get; set; }
        public IProgress<float> Progress { get; set; }
        public IList<string> Scenes { get; set; }
        public bool IgnoreIfAlreadyLoaded { get; set; }

        private SceneConfig(
            LoadSceneMode loadSceneMode, 
            IProgress<float> progress, 
            IList<string> scenes,
            bool ignoreIfAlreadyLoaded)
        {
            LoadSceneMode = loadSceneMode;
            Progress = progress;
            Scenes = scenes;
            IgnoreIfAlreadyLoaded = ignoreIfAlreadyLoaded;
        }
        
        public class SceneConfigBuilder
        {
            private LoadSceneMode _loadSceneMode;
            private IProgress<float> _progress;
            private IList<string> _scenes;
            private bool _ignoreIfAlreadyLoaded;

            public SceneConfigBuilder SetScenes(IList<string> scenes)
            {
                _scenes = scenes.ToList();
                return this;
            }

            public SceneConfigBuilder SetLoadSceneMode(LoadSceneMode loadSceneMode)
            {
                _loadSceneMode = loadSceneMode;
                return this;
            }

            public SceneConfigBuilder SetProgress(IProgress<float> progress)
            {
                _progress = progress;
                return this;
            }
            
            public SceneConfigBuilder SetIgnoreIfAlreadyLoaded(bool ignoreIfAlreadyLoaded)
            {
                _ignoreIfAlreadyLoaded = ignoreIfAlreadyLoaded;
                return this;
            }

            public SceneConfig Build()
            {
                return new SceneConfig(
                    loadSceneMode: _loadSceneMode, 
                    progress: _progress,
                    scenes: _scenes,
                    ignoreIfAlreadyLoaded: _ignoreIfAlreadyLoaded);
            }
        }
    }
}