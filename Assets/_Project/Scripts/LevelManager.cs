using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scenes to Load")]
    [SerializeField] private SceneField _persistentGameplay;
    [SerializeField] private SceneField _levelScene;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistentGameplay));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene));
    }
    
}
