using Mirror;
using TMPro;
using UnityEngine;

public class PlayerUsername : NetworkBehaviour
{
    [SyncVar(hook=nameof(OnNameChanged))]
    public string Username;
    public static string LocalUsername;
    public TMP_InputField UsernameInputField;
    public TextMeshProUGUI UsernameText;

    public override void OnStartLocalPlayer()
    {
        CmdSetUsername(LocalUsername);
    }
    [Command]
    void CmdSetUsername(string newName)
    {
        Username = newName;
    }
    void OnNameChanged(string oldName,string newName)
    {
        UsernameText.text = newName;
    }
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            UsernameText.transform.forward = Camera.main.transform.forward;
        }
    }
}