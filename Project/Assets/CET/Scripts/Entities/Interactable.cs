using System.ComponentModel.Design;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : Activatable
{
    [HideInInspector] public PlayerController CurrentPlayer;

    public Transform UIAnchor;

    public string InteractionText;

    public UnityEvent OnInteractEvent;

    public virtual void Interact()
    {
        OnInteractEvent.Invoke();
    }
}
