using UnityEngine;

public class Keypad : Entity
{
    public BaseLock Lock;

    public Renderer EmissionRenderer;

    public AudioSource AudioSource;

    public Texture ActiveEmissionTexture;
    public Texture InactiveEmissionTexture;

    void Awake()
    {
        Lock.LockStateChanged.AddListener(state => SetState(state));
    }

    public void SetState(bool value)
    {
        if (AudioSource != null && value)
        {
            AudioSource.Play();
        }
        if (EmissionRenderer != null){
            EmissionRenderer.material.SetTexture("_EmissionMap", value ? ActiveEmissionTexture : InactiveEmissionTexture);
        }
    }
}
