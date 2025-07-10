using UnityEngine;
using UnityEngine.Analytics;

/*
    * InteractableMirrorJoint.cs
    * This script allows the player to interact with a mirror joint in the game.
    * When the player carries a mirror (tagged as "Reflector") and interacts with the joint,
    * the mirror is attached to the joint and its layer is set to Default (0).
    * The joint can only be occupied by one mirror at a time.
*/

public class InteractableMirrorJoint : Interactable
{
    PlayerItemHandler playerItemHandler;
    bool occupied = false; // Indicates if the joint is occupied by a mirror

    void Awake()
    {
        // find player item handler in the scene
        playerItemHandler = FindFirstObjectByType<PlayerItemHandler>();
    }

    public override void interact()
    {

        if (playerItemHandler != null && !occupied)
        {
            if (playerItemHandler.GetCarriedObject() != null && playerItemHandler.GetCarriedObject().tag == "Reflector")
            {
                GameObject mirror = playerItemHandler.GetCarriedObject();
                AttachToBone mirrorBoneHandler = playerItemHandler.GetCarriedObject().GetComponent<AttachToBone>();
                if (mirrorBoneHandler != null)
                {
                    mirrorBoneHandler.SetTargetBone(gameObject);
                    playerItemHandler.DropItem(); // Drop the carried object after attaching it to the mirror joint
                                                  // set layer to 0
                    mirror.GetComponent<Rigidbody>().isKinematic = true; // Set the mirror's Rigidbody to kinematic to prevent physics interactions
                    mirror.layer = 7; // Set the layer to Default (0) to ensure it is not interactable by the player
                    occupied = true; // Mark the joint as occupied
                }
            }
        }
    }

    public override void onPowerOff()
    {
        throw new System.NotImplementedException();
    }

    public override void onPowerOn()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
