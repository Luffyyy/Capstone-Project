using System;
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
    private Dictionary<string, int> VariableDefs = new();
    private Dictionary<string, object> Variables = new();

    public Button ExecuteButton;

    private Coroutine executionRoutine;

    public VPLStore Store;

    public Transform BlockListContent;

    public Transform VPLZoneContent;

    public GameObject DeleteZone;

    public List<BlockTray> Trays => Helpers.GetComponentsInChildren<BlockTray>(VPLZoneContent);

    [HideInInspector] public List<BlockDefinition> DefinedBlocks;

    [HideInInspector]
    public BlockNode Root;

    public GameObject TrayPrefab;

    public bool IsActive;

    public UnityEvent<string, string> OnVariableNameChanged;

    public UnityEvent<string, bool> OnVariableDefinitionChanged;

    public TerminalInteractable ConnectedTo;

    public GameObject CategoryText;

    public List<string> ConsoleLog = new();
    public TextMeshProUGUI ConsoleText;
    public ScrollRect ConsoleScroll;

    public void PrintToConsole(string msg)
    {
        ConsoleLog.Add($"[{DateTime.Now:T}]: {msg}");
        if (ConsoleLog.Count > 50)
        {
            ConsoleLog.RemoveAt(0);
        }
        UpdateConsole();
    }
    
    public void UpdateConsole()
    {
        StartCoroutine(AsyncUpdateConsole());
    }

    IEnumerator AsyncUpdateConsole()
    {
        ConsoleText.text = string.Join("\n", ConsoleLog);

        yield return new WaitForSeconds(0.1f);
        Canvas.ForceUpdateCanvases(); 
        ConsoleScroll.verticalNormalizedPosition = 0;

        if (ConnectedTo.isServer)
        {
            yield return new WaitForSeconds(0.5f); // Wait a bit before sending to others
            ConnectedTo.SendConsoleMessageToPeers(ConsoleLog);
        }
    }

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
            if (kv.Value > 0)
            {
                root.VariableDefs.Add(new() {
                    Key = kv.Key,
                    Value = kv.Value.ToString()
                });
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

        VariableDefs = new();

        foreach (var pair in root.VariableDefs)
        {
            VariableDefs[pair.Key] = int.Parse(pair.Value);
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

    public void AddBlocksToMenu()
    {
        var dict = Store.GetCategorizedDefinitions();
        var pairs = dict.ToList().OrderBy((a) => a.Key);

        foreach (var catDefs in pairs)
        {
            var catText = Instantiate(CategoryText, BlockListContent);
            catText.GetComponent<TextMeshProUGUI>().SetText(catDefs.Key.ToString());
                
            foreach (var def in catDefs.Value)
            {
                if (def.DefaultBlock || DefinedBlocks.Contains(def))
                {
                    var blockPrefab = Store.GetPrefabForDefinition(def);
                    if (blockPrefab != null)
                    {
                        var spawned = Instantiate(blockPrefab, BlockListContent);
                        spawned.GetComponent<DraggableBlock>().IsFake = true;
                        spawned.SetDefinition(def);
                    } else
                    {
                        print($"Couldn't find prefab of {def.Name}: {def.PrefabName}");
                    }
                }
            }
        }
    }

    public List<string> GetVariableNames()
    {
        List<string> names = new();
        foreach (var kv in VariableDefs)
        {
            if (kv.Value > 0)
            {
                names.Add(kv.Key);
            }
        }
        return names;
    }

    public string GetVariableName()
    {
        int i = 0;
        while (VariableDefs.TryGetValue("v"+i, out int num) && num > 0) {
            i++;
        }

        return "v"+i;
    }

    public string GetAndDefineVariableName()
    {
        string res = GetVariableName();
        DefineVariable(res);

        return res;
    }
    
    public void UndefineVariable(string name, bool silent=false)
    {
        if (VariableDefs.ContainsKey(name))
        {
            VariableDefs[name] = Math.Max(0, VariableDefs[name]-1);
        }
        // print("Undefine var " + name + $" ({VariableDefs[name]})");

        if (!silent)
        {
            OnVariableDefinitionChanged.Invoke(name, false);
        }
    }

    public void DefineVariable(string name, bool silent=false)
    {
        if (!VariableDefs.ContainsKey(name))
        {
            VariableDefs[name] = 0;
        }
        VariableDefs[name]++;

        // print("Define var " + name+ $" ({VariableDefs[name]})");

        if (!silent)
        {
            OnVariableDefinitionChanged.Invoke(name, true);
        }    
    }

    public void SetVariableName(string oldName, string newName=null)
    {
        int count = 0;

        if (VariableDefs.ContainsKey(oldName))
        {
            count = VariableDefs[oldName];
            VariableDefs[oldName] = 0;
        }
        if (!VariableDefs.ContainsKey(newName))
        {
            VariableDefs[newName] = 0;
        }

        VariableDefs[newName] += count;

        // print("Set var name " + newName+ $" ({VariableDefs[newName]})");
        OnVariableNameChanged.Invoke(oldName, newName);
    }

    /**
        Executes all trays on the VPLZone
    */
    public void Execute()
    {
        if (executionRoutine != null)
        {
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
        PrintToConsole("Executing... ");
        foreach (var tray in Trays)
        {
            yield return tray.Execute();
            PrintToConsole("Execution complete.");
        }
        Cleanup();
    }

    public void Cleanup()
    {
        var button = ExecuteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Variables.Clear();
        button.text = "Execute";
        executionRoutine = null;
    }

    public void SetVariable(string str, object obj)
    {
        Variables[str] = obj;
        if (!VariableDefs.ContainsKey(str))
        {
            VariableDefs[str] = 1;
        }
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
