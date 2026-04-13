using Mirror;
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
        if (NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                NetworkManager.singleton.StopHost();
            }
            {
                NetworkManager.singleton.StopServer();
            }
        } else
        {
            NetworkManager.singleton.StopClient();
        }

    }
}
