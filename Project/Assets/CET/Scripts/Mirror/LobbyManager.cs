using Mirror;
using UnityEngine;
using UnityEngine.Events;

public enum ReadyState
{
    Offline,
    Unready,
    Ready
}

public class LobbyManager : NetworkBehaviour
{
    [Scene, Tooltip("Which scene to send player from here")]
    public string destinationScene;

    public SyncList<ReadyState> ReadyStates = new(){ ReadyState.Offline, ReadyState.Offline };

    public static LobbyManager Instance;

    public LobbyScreen Screen;

    void Awake()
    {
        Instance = this;
        ReadyStates.OnSet += OnReadyPlayersChanged;
    }

    public override void OnStartServer()
    {
        LobbySetup();
    }

    public override void OnStartClient()
    {
        Screen.PlayerMenu.SetActive(true);
        LobbySetup();
    }

    void LobbySetup()
    {
        var networkDiscovery = NetworkManager.singleton.GetComponent<NewNetworkDiscovery>();

        Screen.ServerName.text = networkDiscovery.ServerName;
        Screen.ReadyPlayerTexts[0].text = GetReadyText(0);
        Screen.ReadyPlayerTexts[1].text = GetReadyText(1);
        SetServerStatus();
    }

    string GetReadyText(int i)
    {
        return ReadyStates[i] switch
        {
            ReadyState.Offline => "Waiting...",
            ReadyState.Unready => "Not Ready",
            ReadyState.Ready => "Ready",
            _ => "Invalid Ready State!!"
        };
    }

    void OnReadyPlayersChanged(int i, ReadyState oldValue)
    {
        Screen.ReadyPlayerTexts[i].text = GetReadyText(i);
        SetServerStatus();
    }

    void SetServerStatus()
    {
        if (ReadyStates[0] == ReadyState.Offline || ReadyStates[1] == ReadyState.Offline)
        {
            Screen.ServerStatus.text = "Waiting for Players to Join...";
        } else if (ReadyStates[0] == ReadyState.Unready || ReadyStates[1] == ReadyState.Unready)
        {
            Screen.ServerStatus.text = "Waiting for Players to Ready Up...";
        } else
        {
            Screen.ServerStatus.text = "Starting...";
        }
    }

    [Command(requiresAuthority=false)]
    public void SetReady(bool state, NetworkConnectionToClient sender=null)
    {
        if (sender != null)
        {
            ReadyStates[sender.identity.GetComponent<Player>().PlayerIndex] = state ? ReadyState.Ready : ReadyState.Unready;
        }

        if (ReadyStates[0] == ReadyState.Ready && ReadyStates[1] == ReadyState.Ready)
        {
            NewNetworkManager.singleton.ChangeLevel(destinationScene);
        }
    }
}
