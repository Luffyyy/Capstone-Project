using UnityEngine;
using Mirror;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;

public enum CollectableType {Book,Badge,Photograph,Crystal}
public class Collectable : Interactable
{
    public Sprite Thumbnail;
    public Sprite SpriteToShow;
    public CollectableType Type;
    [ClientRpc]
    public override void ClientInteract()
    {
        base.ClientInteract();
        Collect();
    }
    [Command(requiresAuthority=false)]
    public override void CmdInteract(NetworkConnectionToClient sender=null)
    {
        base.CmdInteract(sender);
        if(isClient)
        {;
            ClientInteract();
        }
        if (!isClient)
        {
            ClientInteract();
            ServerHUD.Instance.ShowCollectable(Thumbnail);
        }
        StartCoroutine(DestroyNextFrame());
    }
    [Server]
    private IEnumerator DestroyNextFrame()
    {
        yield return null;
        NetworkServer.Destroy(gameObject);
    }
    private void Collect()
    {
        
        ControlJournal.Instance.ApplyImage(Type);
        InteractionMenu.Instance.Paper.sprite = SpriteToShow;
        if ( ServerHUD.Instance != null)
        {
            ServerHUD.Instance.ShowCollectable(Thumbnail);   
        }
    }
}
