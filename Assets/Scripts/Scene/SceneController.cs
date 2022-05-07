using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Defines all scene-related actions.
/// </summary>
public class SceneController : Singleton<SceneController>
{
    public string currentScene { get; private set; }

    private void Start()
    {
        currentScene = "Main Menu";
        LoadScene(currentScene);
        GameStateManager.Instance.UpdateState(GameStateManager.GameState.RUNNING);
    }

    /// <summary>
    /// Loads the scene with the given name.
    /// </summary>
    public void LoadScene(string scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        StartCoroutine(SceneProgress(ao, scene, 0));
        currentScene = scene;
    }

    /// <summary>
    /// Unloads the scene with the given name.
    /// </summary>
    public void UnloadScene(string scene)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);
        StartCoroutine(SceneProgress(ao, scene, 1));
    }

    /// <summary>
    /// Unloads the current scene (Boot cannot be unloaded this way).
    /// </summary>
    public void UnloadCurrentScene()
    {
        UnloadScene(currentScene);
    }

    /// <summary>
    /// Use to track the progress of loading a scene.
    /// </summary>
    private IEnumerator SceneProgress(AsyncOperation ao, string scene, int type)
    {
        if (ao == null)
        {
            Debug.LogError(string.Format("Unable to {0} {1}", type == 0 ? "load" : "unload", scene));
            yield break;
        }

        while (!ao.isDone)
        {
            Debug.Log(string.Format("{0} {1} in progress: {2}%", type == 0 ? "Loading" : "Unloading", scene, Mathf.Clamp(ao.progress / 0.9f, 0, 1) * 100));
            yield return null;
        }
    }
}
