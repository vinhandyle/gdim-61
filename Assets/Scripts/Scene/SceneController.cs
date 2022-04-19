using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Defines all scene-related actions.
/// </summary>
public class SceneController : Singleton<SceneController>
{
    private string currentScene = "Main Menu";

    private void Start()
    {
        LoadScene(currentScene);
        GameStateManager.Instance.UpdateState(GameStateManager.GameState.RUNNING);
    }

    /// <summary>
    /// Loads the scene with the given name.
    /// </summary>
    public void LoadScene(string scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        StartCoroutine(SceneProgress(ao, scene));
        currentScene = scene;
    }

    /// <summary>
    /// Unloads the scene with the given name.
    /// </summary>
    public void UnloadScene(string scene)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);
        StartCoroutine(SceneProgress(ao, scene));
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
    private IEnumerator SceneProgress(AsyncOperation ao, string scene)
    {
        if (ao == null)
        {
            Debug.LogError("Unable to load " + scene);
            yield break;
        }

        while (!ao.isDone)
        {
            Debug.Log("Loading " + scene + " in progress: " + Mathf.Clamp(ao.progress / 0.9f, 0, 1) * 100 + "%");
            yield return null;
        }
    }
}
