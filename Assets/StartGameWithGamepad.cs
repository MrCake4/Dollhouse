using UnityEngine;

/*
*   This needs to be deprecated ASAP.
*   It is only used to start the game with a gamepad.
*   The GamepadManager should handle all gamepad-related functionality.
*   However, I dont  have time anymore to get fond with Unity Events.
*/

public class StartGameWithGamepad : MonoBehaviour
{
    LevelManager mainMenuManager;

    void Awake()
    {
        mainMenuManager = FindAnyObjectByType<LevelManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            mainMenuManager.StartGame();
        }
    }
}
