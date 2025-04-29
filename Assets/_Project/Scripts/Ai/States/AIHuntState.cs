using UnityEngine;

public class AIHuntState : AIBaseState
{
    public RoomContainer currentTargetRoom;
    Transform currentTargetWindow;
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 3");

  
    }
    
    public override void onUpdate(AIStateManager ai){
        if(currentTargetRoom == null) ai.switchState(ai.patrolState);

        currentTargetWindow = currentTargetRoom.windowAnchorPoints[0];

        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            currentTargetWindow.position,
            Time.deltaTime * ai.moveSpeed // move speed
        );
    }

    public override void resetVariables(){}

    public void setCurrentTargetRoom(RoomContainer targetRoom){
        this.currentTargetRoom = targetRoom;
    }
}
