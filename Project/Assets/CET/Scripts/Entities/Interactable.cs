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

    // Server countpart of Interact, used to run things on the server
    [Command(requiresAuthority=false)]
    public virtual void CmdInteract(NetworkConnectionToClient sender=null)
    {
        OnInteractEvent.Invoke();
        TargetInteract(sender);
    }

    [TargetRpc]
    public virtual void TargetInteract(NetworkConnectionToClient target)
    {
        
    }
}
