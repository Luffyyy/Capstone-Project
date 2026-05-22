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
        PlayerMenu.SetActive(false); //Gets activated by LobbyManager
        var player = NetworkClient.localPlayer?.GetComponent<Player>();

        if (player != null)
        {
            PlayerColor.value = player.ColorIndex;
            PlayerEmotion.value = player.EmotionIndex;
        }
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
