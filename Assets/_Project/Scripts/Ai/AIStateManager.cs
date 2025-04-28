using UnityEngine;

/*
* Dolly, FINITE STATE MACHINE SETUP
*
* =================================
*
* Abstract script defines functionality of one state
* States are declared as seperate Scripts / Classes
* The Manager manages the states
* States change inside the State classes
*/

public class AIStateManager : MonoBehaviour
{

    // ALL STATES DECLARED HERE
    AIBaseState currentState;
    public AIIdleState idleState = new AIIdleState();
    public AIPatrolState patrolState = new AIPatrolState();

    // GAME OBJECTS
    public Transform spawn;

    void Start()
    {
        // initial state
        currentState = idleState;
        currentState.enterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.onUpdate(this);
    }

    // switches the state
    public void switchState(AIBaseState state) {
        currentState = state;
        currentState.enterState(this);
    }
}
