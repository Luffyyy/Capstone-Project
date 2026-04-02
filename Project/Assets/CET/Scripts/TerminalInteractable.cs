using Mirror;
using UnityEngine;

public class TerminalInteractable : Interactable
{
    public VPLZone VPLEditMenuPrefab;

    [HideInInspector]
    [SyncVar]
    public VPLZone OwnedVPLZone;

    private VPLMenu VPLMenu;

    void Start()
    {
        VPLMenu = MenuManager.Instance.GetMenu<VPLMenu>("VPLMenu");
        if (isServer)
        {
            var go = Instantiate(VPLEditMenuPrefab, VPLMenu.transform);
            NetworkServer.Spawn(go.gameObject);
            OwnedVPLZone = go;
        }
    }

    public override void OnStartClient()
    {
        VPLMenu = MenuManager.Instance.GetMenu<VPLMenu>("VPLMenu");
        if (OwnedVPLZone != null)
        {
            VPLMenu.AddZone(OwnedVPLZone, netId);
        }
    }

    public override void Interact()
    {
        MenuManager.Instance.OpenMenu("VPLMenu");
        VPLMenu.OpenVPLZone(netId);
    }
}
