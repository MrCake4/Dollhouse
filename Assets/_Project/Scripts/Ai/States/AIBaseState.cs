using UnityEngine;

public abstract class AIBaseState
{
    public abstract void enterState(AIStateManager ai);

    public abstract void onUpdate(AIStateManager ai);

    public abstract void exitState(AIStateManager ai);

    public abstract void resetVariables(AIStateManager ai);
}
