using UnityEngine;

// SCRIPT DOES NOTHING RN

public class LoadNextLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the next level here
            // For example, using SceneManager.LoadScene("NextLevelName");
            Debug.Log("Loading next level...");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
