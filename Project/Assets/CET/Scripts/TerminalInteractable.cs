using System.Collections.Generic;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalInteractable : Interactable
{
    public VPLZone VPLEditMenuPrefab;

    [HideInInspector]
    public VPLZone OwnedVPLZone;
    private VPLMenu VPLMenu;
    public List<GameObject> ConnectedObjects = new();

    [HideInInspector, SyncVar(hook=nameof(OnRootChanged))]
    public BlockNode Root;

    protected override void Awake()
    {
        base.Awake();
        SetEmission(IsOn);
        Type = "Terminal";
        VPLMenu = MenuManager.Instance.GetMenu<VPLMenu>("VPLMenu");
        var go = Instantiate(VPLEditMenuPrefab, VPLMenu.transform);

        go.ConnectedTo = this;
        OwnedVPLZone = go;

        VPLMenu.AddZone(go, GetInstanceID());
    }

    void OnRootChanged(BlockNode oldRoot, BlockNode newRoot)
    {
        if (isClient && oldRoot != newRoot)
        {
            OwnedVPLZone.LoadFromTree(Root);
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdSendRoot(BlockNode root, bool execute)
    {
        Root = root;
        OwnedVPLZone.LoadFromTree(root, execute);
    }

    public override void Interact()
    {
        MenuManager.Instance.OpenMenu("VPLMenu");
        VPLMenu.OpenVPLZone(GetInstanceID());
    }
}
