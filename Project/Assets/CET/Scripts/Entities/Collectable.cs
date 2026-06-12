using UnityEngine;
using Mirror;
using System.Collections;

public enum CollectableType {Book,Badge,Photograph,Crystal}
public class Collectable : Interactable
{
    public Sprite Thumbnail;
    public Sprite SpriteToShow;
    public CollectableType Type;
    void Start()
    {
    }
    [ClientRpc]
    public override void ClientInteract()
    {
        base.ClientInteract();
        ControlJournal.Instance.ApplyImage(Type);
        var dialog = MenuManager.Instance.ShowDialog("CollectableDialog") as CollectableDialog;
        dialog.Show(Thumbnail);
        InteractionMenu.Instance.Paper.sprite = SpriteToShow;
    }
    [Command(requiresAuthority=false)]
    public override void CmdInteract(NetworkConnectionToClient sender=null)
    {
        base.CmdInteract(sender);
        ClientInteract();
        StartCoroutine(DestroyNextFrame());
    }
    [Server]
    private IEnumerator DestroyNextFrame()
    {
        yield return null;
        NetworkServer.Destroy(gameObject);
    }
}
