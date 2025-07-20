using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private SceneField[] _scenesToLoad;
    [SerializeField] private SceneField[] _scenesToUnload;

    private GameObject _player;
    private PlayerStateManager playerState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerState = _player.GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    //Activates when an entity enters the collider
    private void OnTriggerEnter(Collider collision)
    {
        bool turnOffInvincible = true;
        if (collision.gameObject == _player && playerState.getCurrentState != playerState.deadState)
        {
            if (playerState.isInvincible == true)
            {
                turnOffInvincible = false;
            }
            else
            {
                playerState.isInvincible = true;
            }
            
            DestroyAllInLayer(LayerMask.NameToLayer("smallObject"));
            DestroyAllInLayer(LayerMask.NameToLayer("destroyOnLoad"));
            StartCoroutine(HandleSceneTransition());
            if(turnOffInvincible) playerState.isInvincible = false;
        }
    }

    // Destroys all GameObjects in a specific layer
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

    private void LoadScenes()
    {
        for (int i = 0; i < _scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadScene = SceneManager.GetSceneAt(j);
                if (loadScene.name == _scenesToLoad[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScenes()
    {
        for (int i = 0; i < _scenesToUnload.Length; i++)
        {
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == _scenesToUnload[i].SceneName)
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }
    }
    
    private IEnumerator HandleSceneTransition()
    {
        SceneFadeManager.instance.StartFadeOut();

        yield return new WaitUntil(() => !SceneFadeManager.instance.isFadingOut);
    
        LoadScenes();
        UnloadScenes();
    }
}
