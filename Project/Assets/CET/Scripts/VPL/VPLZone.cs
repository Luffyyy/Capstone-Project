using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class VPLZone : MonoBehaviour, IDropHandler
{
    private Dictionary<string, bool> VariableDefs = new();
    private Dictionary<string, object> Variables = new();

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
        print("Executing all block trays...");
        var content = transform.GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            var tr = content.GetChild(i);
            if (tr.TryGetComponent<BlockTray>(out var block))
            {
                block.Execute();
            }
        }

        print("Execution complete, cleaning variables...");
        Variables.Clear();
    }

    public GameObject Tray;
    public void OnDrop(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null && obj.TryGetComponent<DraggableBlock>(out var block))
        {
            if (!block.IsBaseBlock) return;

            var tray = Instantiate(Tray, transform.GetChild(0));
            var trayComp = tray.GetComponent<BlockTray>();
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
