using UnityEngine;
using TMPro;
using Mirror;
using System.Collections;
using System;

public class FrequencyModule : Interactable
{
    public TextMeshProUGUI FrequencyText;
    public float Frequency = 0f;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    public void SetFrequency(float newFrequency)
    {
        Frequency = newFrequency;
    }
    [Command(requiresAuthority = false)]
    public override void CmdInteract(NetworkConnectionToClient sender = null)
    {
        base.CmdInteract(sender);
        PlayFrequency(sender);
    }
    [Server]
    public void PlayFrequency(NetworkConnectionToClient target = null)
    {
        if (!NetworkClient.active)
        {
            AudioSource.PlayOneShot(AudioSource.clip, 6f);
        }
        else
        {
            TargetPlayFrequency(target);
        }
    }
    [TargetRpc]
    void TargetPlayFrequency(NetworkConnectionToClient target)
    {
        AudioSource.PlayOneShot(AudioSource.clip, 6f);
    }
}
