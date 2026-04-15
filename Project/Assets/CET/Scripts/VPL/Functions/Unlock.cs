using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlock", menuName = "VPL/Functions/Unlock")]
public class Unlock : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "Port", Type = "number"},
        new() { Name = "Password", Type = "string"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        foreach (var obj in co)
        {
            var interactable = obj.GetComponent<Activatable>();
            if (interactable != null)
            {
                if((interactable.Password == input[1].ToString()) && (interactable.Port == int.Parse(input[0].ToString())))
                {
                    interactable.SetIsOn(true);
                }
            }
        }
    }
}
