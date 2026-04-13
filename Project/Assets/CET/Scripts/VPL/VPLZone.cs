using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gilzoide.FlexUi;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement), typeof(CanvasGroup))]
public class VPLZone : MonoBehaviour
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
    public BlockNode Root;

    public GameObject TrayPrefab;

    public bool IsActive;

    public UnityEvent<string, string> OnVariableNameChanged;

    public TerminalInteractable ConnectedTo;

    public void ExecuteOnServer()
    {
        ConnectedTo.CmdSendRoot(BuildTree(), true);
        //TODO: send back to clients whether execution was a success
    }

    public BlockNode BuildTree()
    {
        BlockNode root = new();

        foreach (var kv in VariableDefs)
        {
            if (kv.Value)
            {
                root.VariableDefs.Add(kv.Key);
            }
        }

        foreach (var tray in Trays)
        {
            root.Trays.Add(tray.SaveNode());
        }

        return root;
    }

    public void LoadFromTree(BlockNode root, bool execute=false)
    {
        Root = root;
        print($"Load tree {root.Ident}");

        VariableDefs = new();

        foreach (var key in root.VariableDefs)
        {
            VariableDefs[key] = true;
        }

        foreach (var tray in Trays)
        {
            DestroyImmediate(tray.gameObject); // Important since we want to avoid a race condition with the coroutine
        }

        foreach (var trayNode in root.Trays)
        {
            var tray = CreateTray();
            tray.LoadNode(trayNode);
        }

        if (execute)
        {
            Execute();
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
        List<string> names = new();
        foreach (var kv in VariableDefs)
        {
            if (kv.Value)
            {
                names.Add(kv.Key);
            }
        }
        return names;
    }

    public string GetVariableName()
    {
        int i = 0;
        while (VariableDefs.ContainsKey("myvar"+i)) {
            i++;
        }

        string res = "myvar"+i;
        SetVariableName(null, res);

        return "myvar"+i;
    }

    public void SetVariableName(string oldName, string newName)
    {
        if (oldName != null)
        {
            VariableDefs[oldName] = false;
        }
        VariableDefs[newName] = true;

        OnVariableNameChanged.Invoke(oldName, newName);
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
        // var button = ExecuteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        // button.text = "Stop Execution"; //TODO
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

        GetComponent<FlexLayout>().enabled = true;

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
        GetComponent<FlexLayout>().enabled = false;

        IsActive = false;
    }
}
