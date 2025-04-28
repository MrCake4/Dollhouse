using UnityEngine;

public abstract class AIBaseState
{
    public abstract void enterState(AIStateManager ai);

    public abstract void onUpdate(AIStateManager ai);
}
