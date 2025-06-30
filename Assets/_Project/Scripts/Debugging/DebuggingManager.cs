using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebuggingManager : MonoBehaviour
{
    private GameObject[] debugElements;
    private bool isMenuActive = false;
    [SerializeField] private SceneField[] scenes;
    [SerializeField] private SceneField persistentScene;
    private bool changingScene = false;

    void Awake()
    {
        debugElements = GameObject.FindGameObjectsWithTag("Debugging");
        toggleMenu(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftShift))
        {
            isMenuActive = !isMenuActive;
            toggleMenu(isMenuActive);
        }
    }

    private void toggleMenu(bool enable)
    {
        foreach (var obj in debugElements)
        {
            obj.SetActive(enable);
        }
    }

    public void toggleDoll(bool isDollEnabled)
    {
        AIStateManager doll = FindAnyObjectByType<AIStateManager>();
        if (!isDollEnabled)
        {
            doll.enabled = false;
        }
        else
        {
            doll.enabled = true;
        }
    }

    public void LevelSelector(int dropdownIndex)
    {
        if (!changingScene)
        {
            SceneFadeManager.instance.StartFadeOut();
            SceneManager.LoadSceneAsync(scenes[dropdownIndex], LoadSceneMode.Additive);
            SceneFadeManager.instance.StartFadeIn();
            SceneManager.UnloadSceneAsync(GetNonPersistentScene(persistentScene));
            changingScene = true;
        }
    }

    private Scene GetNonPersistentScene(SceneField persistentSceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != persistentSceneName.SceneName && scene.isLoaded)
            {
                return scene;
            }
        }
        return default;
    }
}
