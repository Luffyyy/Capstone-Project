using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VPLZone : NetworkBehaviour, IDropHandler
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

    [SyncVar(hook="OnRootChanged")]
    public BlockNode Root;

    public void SendTreeToServer()
    {
        BlockNode root = new();


        foreach (var tray in Trays)
        {
            root.Trays.Add(tray.SaveNode());
        }

        CmdSendRoot(root);
    }

    [Command(requiresAuthority=false)]
    void CmdSendRoot(BlockNode root)
    {
        Root = root;
    }

    public void LoadFromTree(BlockNode root)
    {
        foreach (var tray in Trays)
        {
            Destroy(tray.gameObject);
        }

        foreach (var trayNode in root.Trays)
        {
            var tray = CreateTray();
            tray.LoadNode(trayNode);
        }
    }

    private BlockTray CreateTray()
    {
        var tray = Instantiate(Tray, VPLZoneContent);
        var trayComp = tray.GetComponent<BlockTray>();
        trayComp.Activated(this);
        trayComp.IsRoot = true;
        trayComp.enabled = true;
        return trayComp;
    }

    void OnRootChanged(BlockNode oldRoot, BlockNode newRoot)
    {
        LoadFromTree(Root);
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
            print("Interrupting Execution Coroutine...");
            StopCoroutine(executionRoutine);
            Cleanup();
            return;
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

    public GameObject Tray;
    public void OnDrop(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null && obj.TryGetComponent<DraggableBlock>(out var block))
        {
            if (!block.IsStackBlock) return;

            var tray = CreateTray();
            obj.transform.SetParent(tray.transform);
            block.GetComponent<BaseBlock>().Activated(this);
        }
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
}
