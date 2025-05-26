using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private SceneField[] _scenesToLoad;
    [SerializeField] private SceneField[] _scenesToUnload;

    [SerializeField] bool activateLoad;

    private GameObject _player;
    bool startedLoading = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == _player)
        {
            LoadScenes();
            UnloadScenes();
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
                Scene loadScene = SceneManager.GetSceneAt(j);
                if (loadScene.name == _scenesToUnload[i].SceneName)
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }
    }
}
