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
* States are declared as seperate Scripts / Classes / Components however you want to call them
* The Manager manages the states
* States change inside the State classes
*
* Some variables need to be reseted after switching a state, if not the variable will stay the same
*/

public class AIStateManager : MonoBehaviour
{

    // ALL STATES DECLARED HERE
    AIBaseState currentState;
    AIBaseState lastState = null;
    public AIIdleState idleState = new();
    public AIPatrolState patrolState = new();
    public AISeekState seekState = new();
    public AIHuntState huntState = new();
    public AIAttackState attackState = new();
    public AIScanState scanState = new();

    // GAME OBJECTS
    [Header("Spawn Points")]
    [Tooltip("Where the AIs idle spawn point is")]
    public Transform idleSpawn;
    [Tooltip("Where the AIs patrol spawn point is")]
    public Transform patrolSpawn;
    public AIRoomScan eye;
    
    // TODO: Pseudo code for eyes
    // [RequiredType] public Eye[] eyes;    // Array List with all available eyes, 0 is left, 1 is right  

    [Header("AI Behaviour")]
    [SerializeField, Range(0,300)] private float idleTime;
    //[SerializeField, Range(0,300)] private float checkRoomTime;
    [Range(0,10)] public float moveSpeed = 10;       // How fast AI goes from room to room
    [SerializeField, Range(1,100)] private int checkWindowsPerPatrol = 4;
    [HideInInspector] public int windowsPatrolled = 0;
    [HideInInspector] public RoomContainer currentTargetRoom = null;
    [HideInInspector] public RoomContainer lastKnownRoom = null;
    [HideInInspector] public Transform currentTargetWindow = null;
    [HideInInspector] public int seekIncrement = 1;
    [HideInInspector] public int currentWindowIndex = 0;
    [HideInInspector] public bool scanDone = false;
    [HideInInspector] public bool isPatroling = false;
    [HideInInspector] public int seekRoomsChecked = 0;

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

    // switches the state, called by other states
    public void switchState(AIBaseState state, bool skipOnEnter) {
        lastState = currentState;
        currentState = state;
        if (!skipOnEnter) currentState.enterState(this);
    }

    // this sets the current target room to where the ai is going to
    public void setCurrentTargetRoom(RoomContainer room){
        this.currentTargetRoom = room;
    }

    // if a player triggers a room this method sets the last known position of the player
    public void setLastKnownRoom(RoomContainer room) {
        this.lastKnownRoom = room;
    }

    // this sets the current target window to wich the ai is going to in the current target room
    public void setCurrentTargetWindow(Transform window){
        this.currentTargetWindow = window;
    }

    // debug stuff for debugging
    public void drawDebugStuff(){
        // draw line to targeted ROOM
        if(currentTargetRoom != null) Debug.DrawLine(this.transform.position, currentTargetRoom.transform.position, Color.green);

        // draw line to targeted WINDOW
        if(currentTargetRoom != null && currentTargetWindow != null) Debug.DrawLine(this.transform.position, currentTargetWindow.transform.position, Color.red);
    }


    // Getter and Setter
    public float getIdleTime => idleTime;
    //public float getCheckRoomTime => checkRoomTime;
    public int getCheckWindowPerPatrol => checkWindowsPerPatrol;
    public AIBaseState getLastState => lastState;
}
