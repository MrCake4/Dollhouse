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

    void Awake()
    {
        debugElements = GameObject.FindGameObjectsWithTag("Debugging");
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
        if (dropdownIndex <= 0) return;
        if (player.getCurrentState == player.deadState) player.SwitchState(player.idleState);
        StartCoroutine(LoadSceneRoutine(dropdownIndex - 1));
        DestroyAllInLayer(LayerMask.NameToLayer("smallObject"));
        DestroyAllInLayer(LayerMask.NameToLayer("destroyOnLoad"));
        toggleMenu(false);
        isMenuActive = false;
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

        var oldScene = GetNonPersistentScene(persistentScene);
        if (oldScene.IsValid())
        {
            var unloadOp = SceneManager.UnloadSceneAsync(oldScene);
            yield return unloadOp;
        }

        var loadOp = SceneManager.LoadSceneAsync(scenes[dropdownIndex], LoadSceneMode.Additive);
        yield return loadOp;

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

    void DestroyAllInLayer(int layer)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                Destroy(obj);
            }
        }
    }

    public void ToggleStammina(bool on)
    {
        if (!on)
        {
            player.staminaSystem.staminaDrainRate = player.staminaSystem.defaultStaminaDrainRate;
            player.staminaSystem.jumpStaminaCost = player.staminaSystem.defaultJumpStaminaCost;
        }
        else
        {
            player.staminaSystem.staminaDrainRate = 0f;
            player.staminaSystem.jumpStaminaCost = 0f;
        }
        
    }
}
