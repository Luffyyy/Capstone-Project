using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
public class Activatable : NetworkBehaviour
{
    [SyncVar] public bool IsOn = false;
    public string Type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIsOn(bool value)
    {
        IsOn = value;
    }
}
