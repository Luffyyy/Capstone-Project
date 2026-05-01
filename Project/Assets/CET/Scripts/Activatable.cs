using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
public class Activatable : NetworkBehaviour
{
    [SyncVar(hook = nameof(CallEmission))] public bool IsOn;
    public Renderer EmissionRenderer;
    public Light targetLight;
    public string Password;
    public int Port;

    public override void OnStartClient()
    {
        base.OnStartClient();
        SetEmission(IsOn);
    }
    private void CallEmission(bool oldValue, bool newValue)
    {
        SetEmission(newValue);
    }
    [Server]
    public virtual void SetIsOn(bool value)
    {
        IsOn = value;
        SetEmission(IsOn);
    }

    public void SetEmission(bool value)
    {
        if (EmissionRenderer != null)
        {
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
