using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : NetworkBehaviour
{
    [HideInInspector]
    public bool IsDedicatedServer;

    public static ConnectionManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        IsDedicatedServer = !isClient;
        HUDManager.Instance.SetCurrentHUD(IsDedicatedServer, isClient);
    }

    public override void OnStartClient()
    {
        HUDManager.Instance.SetCurrentHUD(IsDedicatedServer, isClient);
    }
}