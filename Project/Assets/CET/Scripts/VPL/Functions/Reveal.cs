using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Reveal", menuName = "VPL/Functions/Reveal")]
public class Reveal : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "", Type = "str"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        foreach (var obj in co)
        {
            var interactable = obj.GetComponent<RevealObj>();
            if (interactable != null)
            {
                if(interactable.TextToReveal == input[0].ToString())
                {
                    interactable.SetReveal(true);
                }
            }
        }
    }
}
