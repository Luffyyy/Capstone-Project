using Mirror;
using UnityEngine;
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
        HUDManager.Instance.SetCurrentHUD();
    }

    public override void OnStartServer()
    {
        print("On start server");
        print("On start server");
        print("On start server");
        print("On start server");
        print("On start server");
        IsDedicatedServer = !isClient;
    }
}