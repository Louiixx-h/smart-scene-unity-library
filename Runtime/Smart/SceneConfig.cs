using System;

namespace Com.LuisLabs.SmartScene
{
    /// <summary>
    /// Configuration for loading scenes.
    /// </summary>
    public class SceneConfig
    {
        public IProgress<float> Progress { get; set; }
        public SceneGroupData SceneGroup { get; set; }
        public bool IgnoreIfAlreadyLoaded { get; set; }

        private SceneConfig(
            IProgress<float> progress,
            SceneGroupData sceneGroup,
            bool ignoreIfAlreadyLoaded)
        {
            Progress = progress;
            SceneGroup = sceneGroup;
            IgnoreIfAlreadyLoaded = ignoreIfAlreadyLoaded;
        }

        /// <summary>
        /// Builder class for SceneConfig.
        /// </summary>
        public class SceneConfigBuilder
        {
            private IProgress<float> _progress;
            private SceneGroupData _sceneGroup;
            private bool _ignoreIfAlreadyLoaded;

            public SceneConfigBuilder SetSceneGroup(SceneGroupData sceneGroup)
            {
                _sceneGroup = sceneGroup;
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
                    progress: _progress,
                    sceneGroup: _sceneGroup,
                    ignoreIfAlreadyLoaded: _ignoreIfAlreadyLoaded);
            }
        }
    }
}