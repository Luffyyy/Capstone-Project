using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement), typeof(CanvasGroup))]
public class VPLZone : NetworkBehaviour
{
    private Dictionary<string, bool> VariableDefs = new();
    private Dictionary<string, object> Variables = new();

    public Button ExecuteButton;

    private Coroutine executionRoutine;

    public VPLStore Store;

    public Transform BlockListContent;

    public Transform VPLZoneContent;

    public GameObject DeleteZone;

    public List<BlockTray> Trays => Helpers.GetComponentsInChildren<BlockTray>(VPLZoneContent);

    [HideInInspector]
    [SyncVar(hook="OnRootChanged")]
    public BlockNode Root;

    public GameObject TrayPrefab;

    public bool IsActive;

    public TerminalInteractable ConnectedTo;
    public void ExecuteOnServer()
    {
        SendTreeToServer(true); // Ensure tree is up-to-date on server
        //TODO: send back to clients whether execution was a success
    }

    public void SendTreeToServer(bool execute)
    {
        BlockNode root = new();

        foreach (var tray in Trays)
        {
            root.Trays.Add(tray.SaveNode());
        }

        CmdSendRoot(root, execute);
    }

    [Command(requiresAuthority=false)]
    void CmdSendRoot(BlockNode root, bool execute)
    {
        Root = root;
        if (execute)
        {
            LoadFromTree(Root);
            Execute();
        }
    }

    public void LoadFromTree(BlockNode root)
    {
        print($"Load tree {root.Ident}");
        foreach (var tray in Trays)
        {
            DestroyImmediate(tray.gameObject); // Important since we want to avoid a race condition with the coroutine
        }

        foreach (var trayNode in root.Trays)
        {
            var tray = CreateTray();
            tray.LoadNode(trayNode);
        }
    }

    public BlockTray CreateTray()
    {
        var tray = Instantiate(TrayPrefab, VPLZoneContent);
        var trayComp = tray.GetComponent<BlockTray>();
        trayComp.Activated(this);
        trayComp.IsRoot = true;
        trayComp.enabled = true;
        return trayComp;
    }

    void OnRootChanged(BlockNode oldRoot, BlockNode newRoot)
    {
        if (isClient && oldRoot != newRoot)
        {
            LoadFromTree(Root);
        }
    }

    void Awake()
    {
        foreach (var def in Store.Definitions)
        {
            var blockPrefab = Store.GetPrefabForDefinition(def);
            if (blockPrefab != null)
            {
                var spawned = Instantiate(blockPrefab, BlockListContent);
                spawned.GetComponent<DraggableBlock>().IsFake = true;
                spawned.SetDefinition(def);
                spawned.transform.localScale = Vector3.one;
            } else
            {
                print($"Couldn't find prefab of {def.Name}: {def.PrefabName}");
            }
        }
    }

    public List<string> GetVariableNames()
    {
        return VariableDefs.Keys.ToList();
    }

    public string getVariableName()
    {
        int i = 0;
        while (VariableDefs.ContainsKey("myvar"+i)) { //TODO: do this better
            i++;
        }

        string res = "myvar"+i;
        VariableDefs[res] = true;

        return "myvar"+i;
    }

    /**
        Executes all trays on the VPLZone
    */
    public void Execute()
    {
        if (executionRoutine != null)
        {
            print("Interrupting Previous Execution Coroutine...");
            StopCoroutine(executionRoutine);
            Cleanup();
            // return; TODO: possibly implement stopping from client on server
        }

        executionRoutine = StartCoroutine(VPLCoroutine());
    }


    public IEnumerator VPLCoroutine()
    {
        var button = ExecuteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        button.text = "Stop Execution";
        print("Executing all block trays... ");
        foreach (var tray in Trays)
        {
            yield return tray.Execute();
            print("Execution complete.");
        }
        Cleanup();
    }

    public void Cleanup()
    {
        var button = ExecuteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        print("Cleaning variables...");

        Variables.Clear();
        button.text = "Execute";
        executionRoutine = null;
    }

    public void SetVariable(string str, object obj)
    {
        Variables[str] = obj;
        VariableDefs[str] = true;
    }

    public object GetVariable(string str)
    {
        if (Variables.ContainsKey(str)) {
            return Variables[str];
        } else
        {
            return null;
        }
    }

    public void Show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        GetComponent<LayoutElement>().ignoreLayout = false;

        IsActive = true;
    }

    public void Hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        // I could disable it, but then it won't receive updates with the network behavior
        // CanvasGroup makes it transparent but sadly not totally "invisible"
        GetComponent<LayoutElement>().ignoreLayout = true;

        IsActive = false;
    }
}
