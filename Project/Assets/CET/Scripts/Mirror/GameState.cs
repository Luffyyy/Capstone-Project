using Mirror;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    [SyncVar]
    public bool IsDedicatedServer;

    public static GameState Instance;

    void Awake()
    {
        Instance = this;
    }

    public override void OnStartClient()
    {
        Instance = this;
        HUDManager.Instance.SetCurrentHUD();

        VolumeController.Instance.SetDefaults();
        if (IsDedicatedServer)
        {
            // On phones we will only play UI sounds
            VolumeController.Instance.SetVolume("Music", 0);
            VolumeController.Instance.SetVolume("SFX", 0);
        }
    }

    public override void OnStartServer()
    {
        IsDedicatedServer = !isClient;
        HUDManager.Instance.SetCurrentHUD();
        VolumeController.Instance.SetDefaults();
    }

    public bool InLobby()
    {
        var currLevel = NewNetworkManager.singleton.CurrentLevel ?? "";
        return currLevel.EndsWith("LobbyScene.unity");
    }
}