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
    }

    public override void OnStartServer()
    {
        Screen.LobbySetup();
    }

    public override void OnStartClient()
    {
        Screen.PlayerMenu.SetActive(true);
        Screen.LobbySetup();

        ReadyStates.OnSet += OnReadyPlayersChanged;
    }

    void OnReadyPlayersChanged(int i, ReadyState oldValue)
    {
        Screen.SetReadyStates();
    }

    [Command(requiresAuthority=false)]
    public void CmdSetReady(bool state, NetworkConnectionToClient sender=null)
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
