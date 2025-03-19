using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Com.LuisLabs.SmartScene
{
    /// <summary>
    /// Interface for managing scene groups and persistent scenes.
    /// </summary>
    public interface ISmartSceneManagement
    {
        /// <summary>
        /// Event triggered when loading starts.
        /// </summary>
        Action OnLoadingStart { get; set; }

        /// <summary>
        /// Event triggered when loading ends.
        /// </summary>
        Action OnLoadingEnd { get; set; }

        /// <summary>
        /// List of currently loaded scene groups.
        /// </summary>
        SceneGroupData CurrentSceneGroup { get; }

        /// <summary>
        /// List of currently loaded persistent scenes.
        /// </summary>
        SceneGroupData CurrentPersistentSceneGroup { get; }

        /// <summary>
        /// The currently active scene.
        /// </summary>
        Scene ActiveScene { get; }

        /// <summary>
        /// The number of currently loaded scenes.
        /// </summary>
        int SceneCount { get; }

        /// <summary>
        /// Gets the scene at the specified index.
        /// </summary>
        /// <param name="index">The index of the scene to retrieve.</param>
        /// <returns>The scene at the specified index.</returns>
        Scene GetSceneAt(int index);

        /// <summary>
        /// Switches to a new scene group asynchronously.
        /// </summary>
        /// <param name="sceneConfig">Configuration for loading the scene group.</param>
        /// <returns>An IEnumerator for coroutine support.</returns>
        IEnumerator SwitchSceneGroupAsync(SceneConfig sceneConfig);

        /// <summary>
        /// Loads additional scenes into the current scene group asynchronously.
        /// </summary>
        /// <param name="sceneConfig">Configuration for loading the scenes.</param>
        /// <returns>An IEnumerator for coroutine support.</returns>
        IEnumerator LoadSceneToCurrentGroupAsync(SceneConfig sceneConfig);

        /// <summary>
        /// Loads persistent scenes asynchronously.
        /// </summary>
        /// <param name="sceneConfig">Configuration for loading the persistent scenes.</param>
        /// <returns>An IEnumerator for coroutine support.</returns>
        IEnumerator LoadPersistentSceneAsync(SceneConfig sceneConfig);

        /// <summary>
        /// Unloads a scene from the current scene group asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to unload.</param>
        /// <returns>An IEnumerator for coroutine support.</returns>
        IEnumerator UnloadSceneAsync(string sceneName);

        /// <summary>
        /// Unloads a persistent scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the persistent scene to unload.</param>
        /// <returns>An IEnumerator for coroutine support.</returns>
        IEnumerator UnloadPersistentSceneAsync(string sceneName);
    }
}