using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum DefaultScreens { MainMenu, LoadingScreen };

    //public void Load(int id)
    //{
    //    AsyncOperation loadingScreen = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    //    loadingScreen.completed += (o) =>
    //    {
    //        AsyncOperation level = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
    //    };
    //}

    void Start()
    {
        //StartCoroutine(Load(2));
    }

    public static IEnumerator Load(CustomLevel level, CarData car)
    {
        int lsId = (int)DefaultScreens.LoadingScreen;
        AsyncOperation loadingScreenOperation = SceneManager.LoadSceneAsync(lsId, LoadSceneMode.Additive);

        while (!loadingScreenOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        GameObject contls = GameObject.FindGameObjectWithTag("LoadingScreenControllers");
        LoadingScreenController lsc = contls.GetComponent<LoadingScreenController>();
        lsc.SetLevelName(level.name);

        AsyncOperation targetLevel = SceneManager.LoadSceneAsync(level.id.handle, LoadSceneMode.Additive);
        targetLevel.allowSceneActivation = false;

        while (!targetLevel.isDone)
        {
            int percentage = Convert.ToInt32(targetLevel.progress * 100);
            lsc.SetLoadingProgress(percentage);

            if (targetLevel.progress >= 0.9f)
            {
                lsc.SetLoaded(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    targetLevel.allowSceneActivation = true;
                }
            }
            yield return null;
        }

        targetLevel.completed += o =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.UnloadSceneAsync((int)DefaultScreens.LoadingScreen);
        };

        yield return null;
    }

    public static IEnumerator Load(int id)
    {
        int lsId = (int)DefaultScreens.LoadingScreen;
        AsyncOperation loadingScreenOperation =  SceneManager.LoadSceneAsync(lsId, LoadSceneMode.Additive);

        while(!loadingScreenOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        GameObject contls = GameObject.FindGameObjectWithTag("LoadingScreenControllers");
        LoadingScreenController lsc = contls.GetComponent<LoadingScreenController>();

        AsyncOperation targetLevel =  SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
        targetLevel.allowSceneActivation = false;

        while (!targetLevel.isDone)
        {
            int percentage = Convert.ToInt32(targetLevel.progress * 100);
            lsc.SetLoadingProgress(percentage);

            if (targetLevel.progress >= 0.9f)
            {
                lsc.SetLoaded(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    targetLevel.allowSceneActivation = true;
                }
            }
            yield return null;
        }

        targetLevel.completed += o =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.UnloadSceneAsync((int)DefaultScreens.LoadingScreen);
        };

        yield return null;
    }
}
