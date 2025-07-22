using UnityEngine;

public class SceneEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    GameObject player;
    CameraEffects cameraEffects;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
        cameraEffects = FindAnyObjectByType<CameraEffects>();
    }

    void Start()
    {
        cameraEffects.Reset();
        SceneFadeManager.instance.StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
