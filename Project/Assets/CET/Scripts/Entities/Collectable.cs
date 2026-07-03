using UnityEngine;
using Mirror;
using System.Collections;

public enum CollectableType {
    Book,
    Badge,
    Photograph,
    Crystal
}

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
        
        ClientInteract();

        GameState.Instance.Collect(Type);

        if (isServerOnly)
        {
            Collect();
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
        if (ServerHUD.Instance != null)
        {
            ServerHUD.Instance.ShowCollectable(Thumbnail);   
        }
    }
}
