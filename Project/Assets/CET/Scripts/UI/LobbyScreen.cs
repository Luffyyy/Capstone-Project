using Mirror;
using TMPro;
using UnityEngine;

public class LobbyScreen : HUDBase
{
    public bool IsReady;

    public TextMeshProUGUI ServerName;
    public TextMeshProUGUI ServerStatus;

    public TextMeshProUGUI ReadyText;

    public TextMeshProUGUI[] ReadyPlayerTexts;
    public TMP_Dropdown PlayerColor;
    public TMP_Dropdown PlayerEmotion;

    public GameObject PlayerMenu;
    void Start()
    {
        var networkDiscovery = NetworkManager.singleton.GetComponent<NewNetworkDiscovery>();
        ServerName.text = networkDiscovery.ServerName;

        PlayerMenu.SetActive(false); //Gets activated by LobbyManager
    }

    public void LobbySetup()
    {
        var player = NetworkClient.localPlayer?.GetComponent<Player>();

        if (player != null)
        {
            PlayerColor.value = player.ColorIndex;
            PlayerEmotion.value = player.EmotionIndex;
        }

        SetReadyStates();
    }

    public void SetReadyStates()
    {
        ReadyPlayerTexts[0].text = GetReadyText(0);
        ReadyPlayerTexts[1].text = GetReadyText(1);
        SetServerStatus();
    }

    public void SetServerStatus()
    {
        var readyStates = LobbyManager.Instance.ReadyStates;
        if (readyStates[0] == ReadyState.Offline || readyStates[1] == ReadyState.Offline)
        {
            ServerStatus.text = "Waiting for Players to Join...";
        } else if (readyStates[0] == ReadyState.Unready || readyStates[1] == ReadyState.Unready)
        {
            ServerStatus.text = "Waiting for Players to Ready Up...";
        } else
        {
            ServerStatus.text = "Starting...";
        }
    }


    public string GetReadyText(int i)
    {
        var state = LobbyManager.Instance.ReadyStates[i];
        return state switch
        {
            ReadyState.Offline => "Waiting...",
            ReadyState.Unready => "Not Ready",
            ReadyState.Ready => "Ready",
            _ => "Invalid Ready State!!"
        };
    }

    public void OnPlayerColorChanged()
    {
        NetworkClient.localPlayer.GetComponent<Player>().CmdSetColorIndex(PlayerColor.value);
    }

    public void OnPlayerEmotionChanged()
    {
        NetworkClient.localPlayer.GetComponent<Player>().CmdSetEmotionIndex(PlayerEmotion.value);
    }

    public void OnReadyButtonPressed()
    {
        IsReady = !IsReady;

        ReadyText.text = IsReady ? "Ready" : "Not Ready";

        LobbyManager.Instance.SetReady(IsReady);
    }
}
