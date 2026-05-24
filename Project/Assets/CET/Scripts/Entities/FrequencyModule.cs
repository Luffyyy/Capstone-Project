using UnityEngine;
using TMPro;
using Mirror;
using System.Collections;
using System;

public class FrequencyModule : Interactable
{
    [SyncVar (hook = nameof(OnSoundPlayed))] public bool SoundPlayed;
    public TextMeshProUGUI FrequencyText;
    public float Frequency = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetFrequency(float newFrequency)
    {
        Frequency = newFrequency;
    }

    [TargetRpc]
    public override void TargetInteract(NetworkConnectionToClient target)
    {
        base.TargetInteract(target);
        SoundPlayed = !SoundPlayed;
    }
    private void OnSoundPlayed(bool oldValue, bool newValue)
    {
        if (AudioSource != null && !AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot(AudioSource.clip,6f);
        }
    }
}
