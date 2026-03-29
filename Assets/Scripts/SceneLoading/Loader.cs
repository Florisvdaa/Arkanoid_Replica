using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    private static Scene targetScene;

    public enum Scene
    {
        MainMenuScene,
        LoadingScene,
        GameScene,
    }

    private static Action OnLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene targetScene)
    {
        // Set loader callback action to load the target scene
        OnLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(targetScene));
        };

        // Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    /// <summary>
    /// Triggered after first update in LoaderCallback
    /// Execute the loader callback action which will load the target scene
    /// </summary>
    public static void LoaderCallback()
    {
        if (OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
            return 1f;
    }
}
