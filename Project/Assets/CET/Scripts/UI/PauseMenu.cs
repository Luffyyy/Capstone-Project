using UnityEngine;

public class PauseMenu : MenuBase
{
    public void Resume()
    {
        MenuManager.Instance.CloseCurrentMenu(); //TODO: possibly actually pause the game
    }

    public void OpenSettings()
    {
        MenuManager.Instance.OpenMenu("SettingsMenu");
    }

    public void QuitToMenu()
    {
        if (ConnectionManager.Instance.IsDedicatedServer)
        {
            Mirror.NetworkManager.singleton.StopServer();
        } else if (ConnectionManager.Instance.isClientOnly)
        {
            Mirror.NetworkManager.singleton.StopClient();
        } else
        {
            Mirror.NetworkManager.singleton.StopHost();
        }
    }
}
