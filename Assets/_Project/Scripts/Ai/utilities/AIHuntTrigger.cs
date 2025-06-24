using UnityEngine;

public class AIHuntTrigger : MonoBehaviour
{

    [SerializeField] RoomContainer roomToTrigger;
    [SerializeField] AIStateManager ai;
    [SerializeField] bool triggered = false;
    [SerializeField] bool triggerWhenPlayerCarriesObject = false;
    [SerializeField] GameObject objectToCarry;
    PlayerItemHandler playerItemHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerItemHandler = FindFirstObjectByType<PlayerItemHandler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
    {
            if (triggerWhenPlayerCarriesObject)
            {
                if (objectToCarry != null && playerItemHandler.GetCarriedObject() == objectToCarry)
                {
                    // enable ai when it is not already enabled
                    if (!ai.enabled)
                        ai.enabled = true;
                    triggered = true;
                    roomToTrigger.triggered = true;
                }
            }
            // Trigger the hunt state in the AI
            else
            {
                if (!ai.enabled)
                        ai.enabled = true;
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
