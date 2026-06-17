using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ServerHUD : HUDBase
{
    public LevelFinished levelFinished;
    public static ServerHUD Instance;
    public ShowCollectable CollectibleHUD;
    private void Awake()
    {

        Instance = this;
    }

    public override void Show()
    {
        // Since server has no player, we need to enable the player input on the GlobalHUD
        // Another annoying issue is that unity does not like multiple player inputs and causes tons of issues
        if (GameState.Instance.isServerOnly)
        {
            GetComponent<PlayerInput>().enabled = true;
        }

        base.Show();
    }

    public void PlayFinishedLevelHud()
    {
        levelFinished.Show();
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed && !MenuManager.Instance.IsActive)
        {
            MenuManager.Instance.OpenMenu("PauseMenu");
        }
    }
    public void ShowCollectable(Sprite sprite)
    {
        CollectibleHUD.Show(sprite);
    }
}
