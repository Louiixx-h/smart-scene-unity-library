# Smart Scene Library Documentation

## Overview

The `SmartSceneManagement` library is a utility for managing scene transitions in Unity projects. It provides methods to switch scene groups, load scenes into the current group, and manage persistent scenes asynchronously. The library also includes events for handling the start and end of the loading process.

## Installation

To use the `SmartSceneManagement` library in your Unity project, follow these steps:

1. Go to `Package Manager` on Unity.
2. Click on plus button and Select `Add package from git URL`.
3. Paste the `https://github.com/Louiixx-h/smart-scene-unity.git` and press Add button.

## Usage

### Setting Up

1. **Attach the `SmartSceneManagement` Script:**
   - Attach the `SmartSceneManagement` script to a GameObject in your bootstrapper scene.

2. **Create Scene Groups:**
   - Define scene groups as lists of scene names.
   - Use the `SceneConfig` class to configure scene loading options.

### How to use SmartSceneManagement example

```csharp
using System;
using System.Collections.Generic;
using LuisLabs.SmartScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class SmartSceneLoaderController : MonoBehaviour
    {
        [SerializeField] private List<SceneGroupData> groups;
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Camera mainCamera;
    
        private ISmartSceneManagement _smartSceneManagement;
    
        public void Awake()
        {
            _smartSceneManagement = GetComponent<SmartSceneManagement>();
            _smartSceneManagement.OnLoadingStart += OnLoadingStart;
            _smartSceneManagement.OnLoadingEnd += OnLoadingEnd;
        }
    
        private void OnDestroy()
        {
            _smartSceneManagement.OnLoadingStart -= OnLoadingStart;
            _smartSceneManagement.OnLoadingEnd -= OnLoadingEnd;
        }

        private void Start()
        {
            SwitchSceneGroup(groups[0].name);
        }

        private void OnLoadingStart()
        {
            mainCamera.gameObject.SetActive(true);
            loadingScreen.SetActive(true);
        }
    
        private void OnLoadingEnd()
        {
            mainCamera.gameObject.SetActive(false);
            loadingScreen.SetActive(false);
        }

        public void SwitchSceneGroup(string groupName)
        {
            progressBar.fillAmount = 0;
            var scenes = groups.Find(group => group.name == groupName).scenes;
            var builder = new SceneConfig.SceneConfigBuilder()
                .SetScenes(scenes)
                .SetLoadSceneMode(LoadSceneMode.Additive)
                .SetProgress(new Progress<float>(progress => progressBar.fillAmount = progress));
            var sceneConfig = builder.Build();
            _smartSceneManagement.SwitchSceneGroupAsync(sceneConfig);
        }
    
        public void LoadSceneToCurrentGroup(string sceneName)
        {
            var builder = new SceneConfig.SceneConfigBuilder()
                .SetScenes(new List<string> { sceneName })
                .SetLoadSceneMode(LoadSceneMode.Additive)
                .SetIgnoreIfAlreadyLoaded(true);
            var sceneConfig = builder.Build();
            _smartSceneManagement.LoadSceneToCurrentGroupAsync(sceneConfig);
        }
    
        public void LoadPersistentScene(string sceneName)
        {
            var builder = new SceneConfig.SceneConfigBuilder()
                .SetScenes(new List<string> { sceneName })
                .SetLoadSceneMode(LoadSceneMode.Additive)
                .SetIgnoreIfAlreadyLoaded(true);
            var sceneConfig = builder.Build();
            _smartSceneManagement.LoadPersistentSceneAsync(sceneConfig);
        }
    
        public void UnloadScene(string sceneName)
        {
            _smartSceneManagement.UnloadSceneAsync(sceneName);
        }
    
        public void UnloadPersistentScene(string sceneName)
        {
            _smartSceneManagement.UnloadPersistentSceneAsync(sceneName);
        }
    }
}
```

### Key Methods

- **SwitchSceneGroupAsync(SceneConfig sceneConfig)**
  - Unload the current scene group to load a new group of scenes.
  - Usage:
    ```csharp
    var sceneConfig = new SceneConfig.SceneConfigBuilder()
        .SetScenes(new List<string> { "Scene1", "Scene2" })
        .SetLoadSceneMode(LoadSceneMode.Additive)
        .SetProgress(new Progress<float>(progress => progressBar.fillAmount = progress))
        .Build();
    _smartSceneManagement.SwitchSceneGroupAsync(sceneConfig);
    ```

- **LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig)**
  - Loads additional scenes into the current group.
  - Usage:
    ```csharp
    var sceneConfig = new SceneConfig.SceneConfigBuilder()
        .SetScenes(new List<string> { "AdditionalScene" })
        .SetLoadSceneMode(LoadSceneMode.Additive)
        .SetIgnoreIfAlreadyLoaded(true)
        .Build();
    _smartSceneManagement.LoadSceneToCurrentGroupAsync(sceneConfig);
    ```

- **LoadPersistentSceneAsync(SceneConfig sceneConfig)**
  - Loads scenes that should persist across different scene groups.
  - Usage:
    ```csharp
    var sceneConfig = new SceneConfig.SceneConfigBuilder()
        .SetScenes(new List<string> { "PersistentScene" })
        .SetLoadSceneMode(LoadSceneMode.Additive)
        .SetIgnoreIfAlreadyLoaded(true)
        .Build();
    _smartSceneManagement.LoadPersistentSceneAsync(sceneConfig);
    ```

- **UnloadSceneAsync(string sceneName)**
  - Unloads a scene from the current group.
  - Usage:
    ```csharp
    _smartSceneManagement.UnloadSceneAsync("SceneName");
    ```

- **UnloadPersistentSceneAsync(string sceneName)**
  - Unloads a persistent scene.
  - Usage:
    ```csharp
    _smartSceneManagement.UnloadPersistentSceneAsync("PersistentSceneName");
    ```

## Events

- **OnLoadingStart**
  - Event triggered when a scene loading process starts.
- **OnLoadingEnd**
  - Event triggered when a scene loading process ends.

## Contributing

Feel free to contribute to the project by submitting issues or pull requests. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- C# Language
- C# Coroutines
- Unity Documentation
- [SceneManager](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html) in Unity

This documentation covers the basic usage and setup of the `SmartSceneManagement` library. For more advanced usage, refer to the source code and Unity's official documentation.