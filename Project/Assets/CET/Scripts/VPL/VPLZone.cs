using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VPLZone : MonoBehaviour, IDropHandler
{
    private Dictionary<string, object> Variables;

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
            print(tr);
            if (tr.TryGetComponent<BlockTray>(out var block))
            {
                block.Execute();
            }
        }
    }

    public GameObject Tray;
    public void OnDrop(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null && obj.TryGetComponent<DraggableBlock>(out var block))
        {
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
    }

    public object GetVariable(string str)
    {
        return Variables[str];
    }
}
