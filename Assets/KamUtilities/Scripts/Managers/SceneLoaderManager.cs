﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    /// <summary>
    /// Scenes and their build index
    /// </summary>
    public enum Scenes
    {
        MainMenu = 0,
        Level1 = 1,
        WinScreen = 2,
        LoseScreen = 3,
        EmilrangBoss = 4,
    }

    #region Setup
    private static SceneLoaderManager _instance;

    public static SceneLoaderManager instance
    {
        get
        {
            /*
            if (!created)
            {
                //find if there is a scene manager on scene
                SceneLoaderManager temp = FindObjectOfType<SceneLoaderManager>();
                bool managerOnScene = temp != null;
                bool hadPreviousManager = _instance != null;
                if (!managerOnScene)
                {
                    //there is no scene manager on scene, check if there's a previous instance instead
                    if (!hadPreviousManager)
                    {
                        //there is no instance either, meaning you have to create the instance.
                        _instance = (Instantiate(Resources.Load("SceneManager") as GameObject).GetComponent<SceneLoaderManager>());
                        created = true;
                    }
                }
                else
                {
                    //there is a scene manager on scene, make that the active scene manager
                    _instance = temp;
                    created = true;
                }
                
            }
            */
            return _instance;
        }
    }
    #endregion

    public static Scenes lastScene;
    public static Scenes currentScene;

    public void Awake()
    {
        _instance = this;
    }
    public void LoadScene(Scenes scene)
    {
        lastScene = currentScene;
        currentScene = scene;
        StartCoroutine(LoadSceneAsync(scene, LoadSceneMode.Single));
    }

    public void LoadLevelOne()
    {
        LoadScene(Scenes.Level1);
    }
    
    public void LoadWin()
    {
        LoadScene(Scenes.WinScreen);
    }
    
    public void LoadMainMenu()
    {
        LoadScene(Scenes.MainMenu);
    }

    public void LoadLastScene()
    {
        LoadScene(lastScene);
    }
    public void ReloadScene()
    {
        StartCoroutine(LoadSceneAsync(currentScene, LoadSceneMode.Single));
    }

    public void Quit()
    {
        Debug.Log("Exitted the application");
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(Scenes index, LoadSceneMode mode)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync((int)index, mode);
        //LoadingLogo.i.StartLoad(async);
        int framesCount = 0;
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            framesCount += 1;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Terminó la carga en " + framesCount + " frames.");
        async.allowSceneActivation = true;
    }
}
