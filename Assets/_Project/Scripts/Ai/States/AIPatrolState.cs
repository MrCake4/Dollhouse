using UnityEngine;

public class AIPatrolState : AIBaseState
{
    private Transform currentTarget; // Where Dolly is moving to

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered Patrol State");

        ai.transform.position = new Vector3(ai.patrolSpawn.position.x, ai.patrolSpawn.position.y, ai.patrolSpawn.position.z);

        // Pick first target
        PickNewTarget(ai);
    }

    public override void onUpdate(AIStateManager ai) {
        if (currentTarget == null) return;

        // Move towards the current target
        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            currentTarget.position,
            Time.deltaTime * 2f // move speed
        );

        // Check if Dolly reached the target
        if (Vector3.Distance(ai.transform.position, currentTarget.position) < 0.1f) {
            Debug.Log("Reached a window!");

            // Pick a new target or switch state
            PickNewTarget(ai);
        }
    }

    private void PickNewTarget(AIStateManager ai) {
        // Pick a random room
        RoomContainer randomRoom = ai.rooms[Random.Range(0, ai.rooms.Length)];

        // Pick a random window in that room
        if (randomRoom.windowAnchorPoints.Length > 0) {
            currentTarget = randomRoom.windowAnchorPoints[Random.Range(0, randomRoom.windowAnchorPoints.Length)];
        }
    }
}