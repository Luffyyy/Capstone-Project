using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable", menuName = "VPL/Functions/Enable")]
public class Enable : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "", Type = "string" }
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        foreach (var obj in co)
        {
            var interactable = obj.GetComponent<Activatable>();
            if (interactable != null)
            {
                if(interactable.Type == input[0].ToString())
                {
                    interactable.IsOn = true;
                }
            }
        }
    }
}
