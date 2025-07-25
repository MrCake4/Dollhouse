using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Scenes to Load")]
    [SerializeField] private SceneField _persistentGameplay;
    [SerializeField] private SceneField _levelScene;

    [SerializeField] private GameObject[] objectsToHide;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneFadeManager.instance.StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        HideMenu();

        StartCoroutine(HandleSceneTransition());
    }

    private void HideMenu()
    {
        for (int i = 0; i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(false);
        }
    }


    private IEnumerator HandleSceneTransition()
    {
        SceneFadeManager.instance.StartFadeOut();

        yield return new WaitUntil(() => !SceneFadeManager.instance.isFadingOut);

        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistentGameplay));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene, LoadSceneMode.Additive));
    }
}
