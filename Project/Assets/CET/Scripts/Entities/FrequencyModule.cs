using UnityEngine;
using TMPro;
using Mirror;

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
        FrequencyText.text = $"{Frequency}<size=80%>Hz</size>";
    }
    public override void Interact()
    {
        SoundPlayed = !SoundPlayed;
    }
    private void OnSoundPlayed(bool oldValue, bool newValue)
    {
        if (AudioSource != null && !AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot
            (
                AudioSource.clip,
                6f
            );
        }
    }
}
