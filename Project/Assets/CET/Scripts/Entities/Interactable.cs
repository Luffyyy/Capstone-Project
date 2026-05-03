using System.ComponentModel.Design;
using Mirror;
using UnityEngine;

public class Interactable : Activatable
{
    [HideInInspector] public PlayerController CurrentPlayer;

    public Transform UIAnchor;

    public string InteractionText;

    public virtual void Interact()
    {
        
    }
}
