using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnOn", menuName = "VPL/Functions/TurnOn")]
public class TurnOn : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "Port", Type = "number"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        foreach (var obj in co)
        {
            var interactable = obj.GetComponent<Activatable>();
            if (interactable != null)
            {
                
                if(interactable.Port == int.Parse(input[0].ToString()))
                {
                    interactable.SetIsOn(true);
                }
                
            }
        }
    }
}
