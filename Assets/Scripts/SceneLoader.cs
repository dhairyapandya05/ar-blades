using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{

    private string sceneNameToBeLoaded;
    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading());

    }
    IEnumerator InitializeSceneLoading()
    {
        //First we will load the loading scene here
        yield return SceneManager.LoadSceneAsync("Scene_Loading");
        //Load the actual  scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);

        //this value stops the scene from displaying when it is still loading ...
        asyncSceneLoading.allowSceneActivation = false;

        while (!asyncSceneLoading.isDone)
        {
            Debug.Log("Progress Level : " + asyncSceneLoading.progress * 100 + " %");
            if (asyncSceneLoading.progress >= 0.9f)
            {
                //Finally show the scene
                asyncSceneLoading.allowSceneActivation = true;
            }
            yield return null;
        }


    }

}
