using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "Active Scene: " + SceneManager.GetActiveScene().name + "\n" + "Scene Count: " + SceneManager.sceneCount;

        if (Input.GetKey(KeyCode.B))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                text.text = "No Player Found";
            }
            else
            {
                text.text = "Player Found";
                player.transform.position = FindAnyObjectByType<SceneEntry>().transform.position;
                Camera.main.transform.position = player.transform.position;
            }
        }
    }
}
