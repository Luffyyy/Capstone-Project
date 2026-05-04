using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
public class Activatable : Entity
{
    [SyncVar(hook = nameof(OnIsOnChanged))] public bool IsOn;
    public Renderer EmissionRenderer;
    public Light targetLight;

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnIsOnChanged(IsOn, IsOn);
    }

    protected virtual void Awake()
    {
        // Add a listner to the lock and turns the entity on if no longer locked
        if (TryGetComponent<BaseLock>(out var l)) {
            l.LockStateChanged.AddListener((unlocked) =>
            {
                if (unlocked)
                {
                    SetIsOn(unlocked);
                }
            });
        }
    }

    protected virtual void OnIsOnChanged(bool oldValue, bool newValue)
    {
        SetEmission(newValue);
    }

    [Server]
    public virtual void SetIsOn(bool value)
    {
        IsOn = value;
    }

    public void SetEmission(bool value)
    {
        if (EmissionRenderer != null){
            Material mat = EmissionRenderer.material;
            if(mat != null)
            {
                if (value)
                {
                    mat.EnableKeyword("_EMISSION");
                }
                else
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }
        }
        if (targetLight != null)
        {
            targetLight.enabled = value;
        }
    }
}
