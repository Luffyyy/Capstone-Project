using Mirror;
using TMPro;
using UnityEngine;

public class PlayerSaveData
{
    public string Username;
    public int PlayerIndex;
    public int ColorIndex;
    public int EmotionIndex;
}

public class Player : NetworkBehaviour
{
    [SyncVar, HideInInspector]
    public int PlayerIndex;

    [SyncVar(hook=nameof(OnSetColorIndex))]
    public int ColorIndex;
    [SyncVar(hook=nameof(OnSetEmotionIndex))]
    public int EmotionIndex;

    [SyncVar(hook=nameof(OnSetName))]
    public string Username;
    public TextMeshProUGUI UsernameText;

    public override void OnStartServer()
    {
        OnSetName(Username, Username);
    }

    public override void OnStartClient()
    {
        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.Screen.LobbySetup(); // Update name
        }
    }

    public PlayerSaveData GetData() => new()
    {
        PlayerIndex = PlayerIndex,
        // ColorIndex = ColorIndex,
        EmotionIndex = EmotionIndex,
        Username = Username
    };

    void OnSetColorIndex(int oldVal, int newVal)
    {
        SetColorIndex(newVal);
    }

    void OnSetEmotionIndex(int oldVal, int newVal)
    {
        SetEmotionIndex(newVal);
    }

    void OnSetName(string oldName, string newName)
    {
        UsernameText.text = newName;
    }

    [Command]
    public void CmdSetColorIndex(int colorIndex)
    {
        SetColorIndex(colorIndex);
    }

    [Command]
    public void CmdSetEmotionIndex(int emotionIndex)
    {
        SetEmotionIndex(emotionIndex);
    }

    [Command]
    public void CmdSetUsername(string newName)
    {
        OnSetName(Username, newName);
        Username = newName;
    }

    public void SetColorIndex(int colorIndex=-1)
    {
        if (colorIndex == -1)
        {
            colorIndex = PlayerIndex;
        }
        ColorIndex = colorIndex;
        GetComponent<Rob10ColorManager>().ChangeBodyColor(colorIndex);
    }

    public void SetEmotionIndex(int emotionIndex)
    {
        EmotionIndex = emotionIndex;
        GetComponent<EmotionChanger>().SetEmotionEyes(emotionIndex);
        GetComponent<EmotionChanger>().SetEmotionMouth(emotionIndex);
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            UsernameText.transform.parent.forward = Camera.main.transform.forward;
        }
    }
}
