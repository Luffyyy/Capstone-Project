using UnityEngine;
using Mirror;


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
        base.TargetInteract(target);
        var dialog = MenuManager.Instance.ShowDialog("CollectableDialog") as CollectableDialog;
        dialog.Show(Thumbnail);
        InteractionMenu.Instance.Paper.sprite = SpriteToShow;
        Destroy(gameObject);
    }
}
