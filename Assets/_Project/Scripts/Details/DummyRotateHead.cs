using UnityEngine;

public class DummyRotateHead : MonoBehaviour
{

    PlayerStateManager playerStateManager;
    [SerializeField]GameObject head;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Player state manager component in scene
        playerStateManager = FindFirstObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStateManager != null && head != null)
        {
            if (Vector3.Distance(head.transform.position, playerStateManager.transform.position) > 20f) return; // Prevents unnecessary calculations if the head is too far away

            head.transform.LookAt(playerStateManager.transform.position);
        }
    }
}
