using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

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
    [Range(1,100)] public int checkWindowsPerPatrol = 4;
    [HideInInspector] public RoomContainer currentTargetRoom = null;
    [HideInInspector] public RoomContainer lastKnownRoom = null;
    [HideInInspector] public Transform currentTargetWindow = null;
    [HideInInspector] public int seekIncrement = 0;

    [Header("Game Objects")]
    [Tooltip("Index 0 is equal to most left room on the map, index 1 is the room next to it and so on.")]
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
        drawDebugStuff();
    }

    // switches the state
    public void switchState(AIBaseState state) {
        currentState = state;
        currentState.enterState(this);
    }

    public void setCurrentTargetRoom(RoomContainer room){
        this.currentTargetRoom = room;
    }

    public void setLastKnownRoom(RoomContainer room) {
        this.lastKnownRoom = room;
    }

    public void setCurrentTargetWindow(Transform window){
        this.currentTargetWindow = window;
    }

    public void drawDebugStuff(){
        // draw line to targeted room
        if(currentTargetRoom != null) Debug.DrawLine(this.transform.position, currentTargetRoom.transform.position, Color.green);

        // draw line to targeted window
        if(currentTargetRoom != null && currentTargetWindow != null) Debug.DrawLine(this.transform.position, currentTargetWindow.transform.position, Color.red);
    }
}
