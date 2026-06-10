using UnityEngine;
using Mirror;
using System.Collections;


public class Collectable : Interactable
{
    public Sprite Thumbnail;
    public Sprite SpriteToShow;
    void Start()
    {
    }
    [TargetRpc]
    public override void TargetInteract(NetworkConnectionToClient target)
    {
        Debug.Log("TargetInteract");
        base.TargetInteract(target);
        var dialog = MenuManager.Instance.ShowDialog("CollectableDialog") as CollectableDialog;
        dialog.Show(Thumbnail);
        InteractionMenu.Instance.Paper.sprite = SpriteToShow;
    }
    [Command(requiresAuthority=false)]
    public override void CmdInteract(NetworkConnectionToClient sender=null)
    {
        base.CmdInteract(sender);
        Debug.Log(sender == null ? "SENDER NULL" : "SENDER OK");
        StartCoroutine(DestroyNextFrame());
    }
    [Server]
    private IEnumerator DestroyNextFrame()
    {
        yield return null;
        NetworkServer.Destroy(gameObject);
    }
}
