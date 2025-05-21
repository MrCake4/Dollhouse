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
                    triggered = true;
                    roomToTrigger.triggered = true;
                }
            }
            // Trigger the hunt state in the AI
             else
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
