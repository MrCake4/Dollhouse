using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void onEnter(PlayerStateManager player);                //vor Update
    public abstract void onUpdate(PlayerStateManager player);               //pro Frame
    public abstract void onFixedUpdate(PlayerStateManager player);          //Physik
    public abstract void onExit(PlayerStateManager player);                 //was passiert, wenn aus State rausgeht
}
