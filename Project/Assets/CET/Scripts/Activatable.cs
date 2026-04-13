using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
public class Activatable : NetworkBehaviour
{
    [SyncVar(hook = nameof(CallEmission))] public bool IsOn;
    public Renderer EmissionRenderer;
    public string Type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
