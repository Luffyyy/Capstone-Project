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

    public List<BlockDefinition> DefinedBlocks;

    [ClientRpc]
    public void SendConsoleMessageToPeers(List<string> log)
    {
        if (isServer) return;

        OwnedVPLZone.ConsoleLog = log;
        OwnedVPLZone.UpdateConsole();
    }

    protected override void Awake()
    {
        base.Awake();
        SetEmission(IsOn);
        VPLMenu = MenuManager.Instance.GetMenu<VPLMenu>("VPLMenu");
        var go = Instantiate(VPLEditMenuPrefab, VPLMenu.transform);

        go.ConnectedTo = this;
        OwnedVPLZone = go;
        go.DefinedBlocks = DefinedBlocks;
        go.AddBlocksToMenu();

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
