using System;
using System.Collections;
using Tayx.Graphy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebuggingManager : MonoBehaviour
{
    private GameObject[] debugElements;
    private bool isMenuActive = false;
    [SerializeField] private SceneField[] scenes;
    [SerializeField] private SceneField persistentScene;
    private AIStateManager doll;
    private PlayerStateManager player;
    private bool isGraphyEnabled = true;
    private GraphyManager graphy;

    void Awake()
    {
        debugElements = GameObject.FindGameObjectsWithTag("Debugging");
        graphy = FindFirstObjectByType<GraphyManager>();
        toggleMenu(false);
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
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
        doll = FindAnyObjectByType<AIStateManager>();
        if (doll == null) Debug.Log("No Doll");
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

        StartCoroutine(LoadSceneRoutine(dropdownIndex));
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

    private IEnumerator LoadSceneRoutine(int dropdownIndex)
    {
        SceneFadeManager.instance.StartFadeOut();
        yield return new WaitUntil(() => !SceneFadeManager.instance.isFadingOut);

        var loadOp = SceneManager.LoadSceneAsync(scenes[dropdownIndex], LoadSceneMode.Additive);
        yield return loadOp;

        Scene newScene = SceneManager.GetSceneByName(scenes[dropdownIndex].SceneName);
        if (newScene.IsValid())
            SceneManager.SetActiveScene(newScene);

        var oldScene = GetNonPersistentScene(persistentScene);
        if (oldScene.IsValid() && oldScene.name != newScene.name)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(oldScene);
            yield return unloadOp;
        }

        SceneFadeManager.instance.StartFadeIn();
    }


    public void ToggleInvincibility(bool on)
    {
        if (!on)
        {
            player.isInvincible = false;
        }
        else
        {
            player.isInvincible = true;
        }
    }
}
