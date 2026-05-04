using UnityEngine;

public class Keypad : Entity
{
    public BaseLock Lock;

    public Renderer EmissionRenderer;

    public Texture ActiveEmissionTexture;
    public Texture InactiveEmissionTexture;

    void Awake()
    {
        Lock.LockStateChanged.AddListener(state => SetEmission(state));
    }

    public void SetEmission(bool value)
    {
        if (EmissionRenderer != null){
            EmissionRenderer.material.SetTexture("_EmissionMap", value ? ActiveEmissionTexture : InactiveEmissionTexture);
        }
    }
}
