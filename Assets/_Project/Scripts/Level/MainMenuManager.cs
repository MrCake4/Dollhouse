using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scenes to Load")]
    [SerializeField] private SceneField _persistentGameplay;
    [SerializeField] private SceneField _levelScene;

    [SerializeField] private GameObject[] objectsToHide;

    public void StartGame()
    {
        HideMenu();
        SceneManager.LoadSceneAsync(_persistentGameplay);
        SceneManager.LoadSceneAsync(_levelScene, LoadSceneMode.Additive);
    }

    private void HideMenu()
    {
        for (int i = 0; i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(false);
        }
    }
}
