using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DialogueSay : MonoBehaviour
{
    public List<LineDefinition> LinesToSay;

    [Multiline]
    public List<string> Lines;
    public bool SayOnAwake = true;
    
    public bool SayOnce = true;

    private bool said = false;
    
    void Start()
    {
        if (SayOnAwake)
        {
            Say();
        }
    }

    public void Say()
    {
        // Lines are sent from server to clients
        if (!enabled || !NetworkServer.active || (SayOnce && said)) return;

        DialogueUI.Instance.Interrupt();

        said = true;
        foreach (var line in LinesToSay)
        {
            DialogueUI.Instance.Say(line);
        }
    }
}
