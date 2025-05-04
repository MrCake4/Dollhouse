using UnityEngine;

public abstract class AIBaseState
{
    // is called once when state is enteres
    public abstract void enterState(AIStateManager ai);

    // is called every frame in the state
    public abstract void onUpdate(AIStateManager ai);

    // is called once state is exited inside the state
    public abstract void exitState(AIStateManager ai);

    // is called by other components or the state itself
    // however this is more like a placholder method
    public abstract void resetVariables(AIStateManager ai);
}
