using UnityEngine;

public class SceneEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    GameObject player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
    }

    void Start()
    {
        SceneFadeManager.instance.StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
