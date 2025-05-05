using System;
using UnityEngine;
using UnityEngine.Animations;

public class RoomContainer : MonoBehaviour
{

    public Transform[] windowAnchorPoints;
    public Boolean triggered = false;
    public Boolean checkedRoom = false;
    [SerializeField] AIStateManager ai;
    public int windowCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        windowCount = windowAnchorPoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && !checkedRoom) {
            
            ai.setCurrentTargetRoom(this);
            ai.setLastKnownRoom(this);
            ai.eye.setStartScan(false);
            ai.switchState(ai.huntState,false);
    }
}
}
