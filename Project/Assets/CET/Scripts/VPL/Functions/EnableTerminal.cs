using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableTerminal", menuName = "VPL/Functions/Enable Terminal")]
public class EnableTerminal : VPLFunction
{
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        foreach (var obj in co)
        {
            var interactable = obj.GetComponent<TerminalInteractable>();
            if (interactable != null)
            {
                interactable.IsOn = true;
            }
        }
    }
}
