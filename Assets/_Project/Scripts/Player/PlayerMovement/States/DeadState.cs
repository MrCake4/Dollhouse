using UnityEngine;

public class DeadState : BasePlayerState
{
    private Rigidbody playerRigidbody;
    private CapsuleCollider mainCollider;
    private bool hasLandedAfterDeath = false;
    GameOverManager gameOverManager;

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Player is dead!");

        gameOverManager = player.GetGameOverManager(); 

        playerRigidbody = player.GetComponent<Rigidbody>();
        mainCollider = player.GetComponent<CapsuleCollider>();

        AudioClip[] deathSounds = player.GetComponent<PlayerSoundManager>().deathSounds;
        if (deathSounds.Length > 0) SoundEffectsManager.instance.PlayRandomSoundEffect(deathSounds, player.transform, 1f);

        //if (playerRigidbody != null) playerRigidbody.isKinematic = true;
        //if (mainCollider != null) mainCollider.enabled = false;

        // turn off all lights except the important ones
        if (gameOverManager != null)
        {
            gameOverManager.SaveImportantLightStates(); // Save BEFORE turning off
            gameOverManager.SetLightStates(false);      // Then turn off lights
        }

            // === Animationsentscheidung ===
            if (player.groundCheck.isGrounded)
            {
                if (Random.Range(0, 1) < 0.5)
                {
                    player.animator.SetTrigger("Die");
                }
                else
                {
                    player.animator.SetTrigger("Die2");
                }

                //player.animator.SetTrigger("Die2");

                hasLandedAfterDeath = true; // sofort „tot“ am Boden
                if (playerRigidbody != null) playerRigidbody.isKinematic = true;
                if (mainCollider != null) mainCollider.enabled = false;
            }
            else
            {
                player.animator.SetBool("IsFalling", true);
                player.animator.SetBool("ReachedJumpPeak", true);
                hasLandedAfterDeath = false; // noch in der Luft → beobachten
            }
    }

    public override void onUpdate(PlayerStateManager player) { }

    public override void onFixedUpdate(PlayerStateManager player)
    {
        if (!hasLandedAfterDeath && player.groundCheck.isGrounded)
        {
            Debug.Log("Player hit ground after death.");
            player.animator.SetBool("IsFalling", false); // ⬅️ das hat dir gefehlt!
            player.animator.SetTrigger("HitGround"); // Animation am Boden nach Aufprall
            hasLandedAfterDeath = true;
            if (playerRigidbody != null) playerRigidbody.isKinematic = true;
            if (mainCollider != null) mainCollider.enabled = false;
        }
    }

    public override void onExit(PlayerStateManager player)
    {
        // Optional, falls du jemals aus dem DeadState raus willst
        if (playerRigidbody != null) playerRigidbody.isKinematic = false;
        if (mainCollider != null) mainCollider.enabled = true;

        gameOverManager?.SetLightStates(true); // Spotlight on, other lights off
        gameOverManager?.RestoreImportantLightStates(); // Restore important lights

        player.animator.ResetTrigger("Die");
    }
}
