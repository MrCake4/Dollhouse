using System;
using UnityEngine;
using UnityEngine.Animations;

public class RoomContainer : MonoBehaviour
{

    [Header("AnkerPoints in Room")]
    public Transform[] windowAnchorPoints;
    public Boolean triggered = false;
    public Boolean checkedRoom = false;
    [SerializeField] AIStateManager ai;
    public int windowCount => windowAnchorPoints.Length;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !checkedRoom)
        {
            ai.setCurrentTargetRoom(this);
            ai.setLastKnownRoom(this);
            ai.eye.setStartScan(false);
            ai.switchState(ai.huntState, false);
        }
    }
}
