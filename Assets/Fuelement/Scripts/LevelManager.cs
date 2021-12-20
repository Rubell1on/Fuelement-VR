﻿using System;
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

    public IEnumerator Load(CustomLevel level, CarData car)
    {
        int lsId = (int)DefaultScreens.LoadingScreen;
        AsyncOperation loadingScreenOperation = SceneManager.LoadSceneAsync(lsId, LoadSceneMode.Additive);

        while (!loadingScreenOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        LoadingScreenController lsc = LoadingScreenController.GetInstance();
        lsc?.SetLevelName(level.name);

        AsyncOperation targetLevel = SceneManager.LoadSceneAsync(level.sceneBuiltInId, LoadSceneMode.Additive);
        targetLevel.allowSceneActivation = false;

        targetLevel.completed += o =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene())
                .completed += OnMainMenuUnloaded;
        };

        while (!targetLevel.isDone)
        {
            int percentage = Convert.ToInt32(targetLevel.progress * 100);
            lsc?.SetLoadingProgress(percentage);

            if (targetLevel.progress >= 0.9f)
            {
                lsc?.SetLoaded(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    targetLevel.allowSceneActivation = true;
                }
            }
            yield return null;
        }

        yield return null;

        void OnMainMenuUnloaded(AsyncOperation asyncOperation)
        {
            Scene placeholder = SceneManager.GetAllScenes().ToList().Find(s => s.name.ToLower().Contains("placeholder"));

            if (placeholder.name?.Length > 0)
            {
                SceneManager.UnloadSceneAsync(placeholder.buildIndex);
            }

            SceneManager.UnloadSceneAsync((int)DefaultScreens.LoadingScreen)
                .completed += OnLoadingScreenUnloaded;
        }

        void OnLoadingScreenUnloaded(AsyncOperation obj)
        {
            LevelSettings levelSetup = LevelSettings.GetInstance();

            if (levelSetup)
            {
                levelSetup.SetCar(car);
                levelSetup.levelData = level;
                levelSetup.SetTitle(level.title);
            }
        }
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

    IEnumerator LoadLevel(int levelId, Action<AsyncOperation> asyncOperationCallback = null)
    {
        AsyncOperation targetLevel = SceneManager.LoadSceneAsync(levelId, LoadSceneMode.Additive);

        while(!targetLevel.isDone)
        {
            if (asyncOperationCallback != null)
            {
                asyncOperationCallback(targetLevel);
            }
            
            yield return null;
        }
    }

    IEnumerator UnloadLevel(Scene scene, Action<AsyncOperation> asyncOperationCallback = null)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scene);

        while(!asyncOperation.isDone)
        {
            if (asyncOperationCallback != null)
            {
                asyncOperationCallback(asyncOperation);
            }

            yield return null;
        }
    }

    IEnumerator UnloadLevel(int id, Action<AsyncOperation> asyncOperationCallback = null)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(id);

        while (!asyncOperation.isDone)
        {
            if (asyncOperationCallback != null)
            {
                asyncOperationCallback(asyncOperation);
            }

            yield return null;
        }
    }

    //public IEnumerator Load(CustomLevel level, CarData car)
    //{
    //    yield return StartCoroutine(LoadLoadingScreen());
    //    yield return new WaitForEndOfFrame();

    //    LoadingScreenController lsc = LoadingScreenController.GetInstance();
    //    lsc?.SetLevelName(level.name);

    //    yield return new WaitForEndOfFrame();

    //    yield return StartCoroutine(LoadLevel(level.sceneBuiltInId, targetLevel =>
    //    {
    //        targetLevel.allowSceneActivation = false;
    //        int percentage = Convert.ToInt32(targetLevel.progress * 100);
    //        lsc?.SetLoadingProgress(percentage);

    //        if (targetLevel.progress >= 0.9f)
    //        {
    //            lsc?.SetLoaded(true);
    //            if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                targetLevel.allowSceneActivation = true;
    //            }
    //        }
    //    }));

    //    yield return new WaitForEndOfFrame();

    //    yield return StartCoroutine(UnloadLevel(SceneManager.GetActiveScene()));

    //    yield return new WaitForEndOfFrame();

    //    Scene placeholder = SceneManager.GetAllScenes().ToList().Find(s => s.name.ToLower().Contains("placeholder"));

    //    if (placeholder.name?.Length > 0)
    //    {
    //        yield return StartCoroutine(UnloadLevel(placeholder.buildIndex));
    //    }

    //    yield return StartCoroutine(UnloadLoadingScreen());

    //    LevelSettings levelSetup = LevelSettings.GetInstance();

    //    if (levelSetup)
    //    {
    //        levelSetup.SetCar(car);
    //        levelSetup.levelData = level;
    //        levelSetup.SetTitle(level.title);
    //    }
    //}

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
