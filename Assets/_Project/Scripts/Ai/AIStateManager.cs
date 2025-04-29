using System;
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
*
* State variables need to be reseted after switching a state, if not the variable will stay the same
*/

public class AIStateManager : MonoBehaviour
{

    // ALL STATES DECLARED HERE
    AIBaseState currentState;
    public AIIdleState idleState = new AIIdleState();
    public AIPatrolState patrolState = new AIPatrolState();
    public AISeekState seekState = new AISeekState();
    public AIHuntState huntState = new AIHuntState();
    public AIAttackState attackState = new AIAttackState();

    // GAME OBJECTS
    [Header("Spawn Points")]
    [Tooltip("Where the AIs idle spawn point is")]
    public Transform idleSpawn;
    [Tooltip("Where the AIs patrol spawn point is")]
    public Transform patrolSpawn;

    [Header("AI Behaviour")]
    [Range(0,300)] public float idleTime;
    [Range(0,300)] public float checkRoomTime;
    [Range(0,10)] public float moveSpeed = 10;       // How fast AI goes from room to room

    [Header("Game Objects")]
    public RoomContainer[] rooms;   // contains information about position of the window

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
