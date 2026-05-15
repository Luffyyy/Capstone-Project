using System.Collections.Generic;
using System.Linq;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalInteractable : Interactable
{
    public VPLZone VPLEditMenuPrefab;

    public string PredefinedListVariableName;
    public List<float> PredefinedListVariableValue;

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
        go.transform.SetSiblingIndex(1);

        go.ConnectedTo = this;
        OwnedVPLZone = go;
        go.DefinedBlocks = DefinedBlocks;
        go.AddBlocksToMenu();

        VPLMenu.AddZone(go, GetInstanceID());

        List<object> lst = new();
        foreach (var obj in PredefinedListVariableValue)
        {
            lst.Add(obj);
        }
        OwnedVPLZone.ReadOnlyVariables[PredefinedListVariableName] = lst;
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
