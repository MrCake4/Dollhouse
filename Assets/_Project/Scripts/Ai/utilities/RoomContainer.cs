using System;
using UnityEngine;
using UnityEngine.Animations;

public class RoomContainer : MonoBehaviour
{

    public Transform[] windowAnchorPoints;
    public Boolean triggered = false;
    public Boolean checkedRoom = false;
    [SerializeField] AIStateManager ai;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && !checkedRoom) {
            ai.switchState(ai.huntState);
            ai.huntState.setCurrentTargetRoom(this);
    }
}
}
