using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public enum DefaultScreens { MainMenu, LoadingScreen };

    void Start()
    {
        //StartCoroutine(Load(2));
    }

    public Coroutine LoadMainMenu()
    {
        return StartCoroutine(_LoadMainMenu());
    }

    public IEnumerator _LoadMainMenu()
    {
        yield return StartCoroutine(LoadLevel((int)DefaultScreens.MainMenu));
        yield return StartCoroutine(UnloadLevel(SceneManager.GetActiveScene()));
    }

    IEnumerator LoadLoadingScreen()
    {
        int lsId = (int)DefaultScreens.LoadingScreen;
        yield return StartCoroutine(LoadLevel(lsId));
    }

    IEnumerator UnloadLoadingScreen()
    {
        int lsId = (int)DefaultScreens.LoadingScreen;
        yield return StartCoroutine(UnloadLevel(lsId));
    }

    IEnumerator LoadGameLevel(CustomLevel level)
    {
        yield return StartCoroutine(LoadLoadingScreen());

        LoadingScreenController lsc = LoadingScreenController.GetInstance();
        lsc?.SetLevelName(level.name);

        AsyncOperation targetLevel = SceneManager.LoadSceneAsync(level.sceneBuiltInId, LoadSceneMode.Additive);

        while (!targetLevel.isDone)
        {
            targetLevel.allowSceneActivation = false;
            int percentage = Convert.ToInt32(targetLevel.progress * 100);
            lsc?.SetLoadingProgress(percentage);

            if (targetLevel.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        lsc?.SetLoaded(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        targetLevel.allowSceneActivation = true;
    }

    public IEnumerator LoadLevel(int levelId, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, Action<AsyncOperation> whileLoopCallback = null)
    {
        AsyncOperation targetLevel = SceneManager.LoadSceneAsync(levelId, loadSceneMode);

        while(!targetLevel.isDone)
        {
            if (whileLoopCallback != null)
            {
                whileLoopCallback(targetLevel);
            }
            
            yield return null;
        }
    }

    public IEnumerator UnloadLevel(Scene scene, Action<AsyncOperation> whileLoopCallback = null)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scene);

        while(!asyncOperation.isDone)
        {
            if (whileLoopCallback != null)
            {
                whileLoopCallback(asyncOperation);
            }

            yield return null;
        }
    }

    public IEnumerator UnloadLevel(int id, Action<AsyncOperation> whileLoopCallback = null)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(id);

        while (!asyncOperation.isDone)
        {
            if (whileLoopCallback != null)
            {
                whileLoopCallback(asyncOperation);
            }

            yield return null;
        }
    }

    public Coroutine Load(CustomLevel level, CarData car)
    {
        return StartCoroutine(_Load(level, car));
    }

    IEnumerator _Load(CustomLevel level, CarData car)
    {
        yield return StartCoroutine(LoadGameLevel(level));

        Scene activeScene = SceneManager.GetActiveScene();

        yield return StartCoroutine(UnloadLevel(activeScene));
        yield return StartCoroutine(UnloadAllPlaceholders());
        yield return StartCoroutine(UnloadLoadingScreen());

        LevelSettings levelSetup = LevelSettings.GetInstance();

        if (levelSetup)
        {
            levelSetup.SetCar(car);
            levelSetup.levelData = level;
            levelSetup.SetTitle(level.title);
        }
    }

    IEnumerator UnloadAllPlaceholders()
    {
        Scene[] placeholders = SceneManager.GetAllScenes().ToList().Where(s => s.name.ToLower().Contains("placeholder")).ToArray();

        foreach (Scene s in placeholders)
        {
            yield return StartCoroutine(UnloadLevel(s.buildIndex));
        }
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
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene())
                .completed += ao =>
                {
                    SceneManager.UnloadSceneAsync((int)DefaultScreens.LoadingScreen);
                };
            
        };

        yield return null;
    }
}
