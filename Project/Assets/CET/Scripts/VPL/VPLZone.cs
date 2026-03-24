using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VPLZone : MonoBehaviour, IDropHandler
{
    private Dictionary<string, bool> VariableDefs = new();
    private Dictionary<string, object> Variables = new();

    public Button ExecuteButton;

    private Coroutine executionRoutine;

    public VPLStore Store;

    public Transform BlocksContent;

    void Awake()
    {
        foreach (var def in Store.Definitions)
        {
            var blockPrefab = Store.GetPrefabForDefinition(def);
            if (blockPrefab != null)
            {
                var spawned = Instantiate(blockPrefab);
                spawned.transform.SetParent(BlocksContent);
                blockPrefab.GetComponent<DraggableBlock>().IsFake = true;
                blockPrefab.SetDefinition(def);
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
        var content = transform.GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            var tr = content.GetChild(i);
            if (tr.TryGetComponent<BlockTray>(out var tray)) {
                yield return tray.Execute();
            }
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

            var tray = Instantiate(Tray, transform.GetChild(0));
            var trayComp = tray.GetComponent<BlockTray>();
            trayComp.Activated(this);
            trayComp.IsRoot = true;
            trayComp.enabled = true;

            tray.transform.position = obj.transform.position;
            obj.transform.SetParent(tray.transform);
            block.IsNew = false;
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
