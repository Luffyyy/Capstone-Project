using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GlobalHUD : HUDBase
{
    void Awake()
    {
        // Since server has no player, we need to enable the player input on the GlobalHUD
        // Another annoying issue is that unity does not like multiple player inputs and causes tons of issues
        if (GameState.Instance.isServerOnly)
        {
            GetComponent<PlayerInput>().enabled = true;
        }
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuManager.Instance.OpenMenu("PauseMenu");
        }
    }
    public void OpenLevelFinishedUI()
    {
          MenuManager.Instance.OpenMenu("LevelFinished");
    }
}