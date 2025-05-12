using UnityEngine;

public class AIHuntTrigger : MonoBehaviour
{

    [SerializeField] RoomContainer roomToTrigger;
    [SerializeField] AIStateManager ai;
    [SerializeField] bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            // Trigger the hunt state in the AI
            if (ai != null)
            {
                triggered = true;
                roomToTrigger.triggered = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
