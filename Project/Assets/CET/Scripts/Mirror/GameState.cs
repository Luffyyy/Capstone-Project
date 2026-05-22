using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    }

    public override void OnStartServer()
    {
        IsDedicatedServer = !isClient;
        HUDManager.Instance.SetCurrentHUD();
    }

    public bool InLobby()
    {
        var currLevel = NewNetworkManager.singleton.CurrentLevel ?? "";
        return currLevel.EndsWith("LobbyScene.unity");
    }
}