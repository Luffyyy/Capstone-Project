using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Guess", menuName = "VPL/Functions/Guess")]
public class Guess : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "letter", Type = "str"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        string letter = input[0].ToString().ToLower();
        foreach (var obj in co)
        {
            if (obj.TryGetComponent<RevealObj>(out var reveal) && obj.transform.parent.gameObject.activeInHierarchy)
            {
                if(reveal.TextToReveal == letter)
                {
                    reveal.SetReveal(true);
                }
            }
        }
    }
}
