using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetChannel", menuName = "VPL/Functions/SetChannel")]
public class SetChannel : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "number", Type = "num"}
    };
    public override void Execute(params object[] input)
    {
        var co = Zone.ConnectedTo.ConnectedObjects;
        int num = int.Parse(input[0].ToString());
        foreach (var obj in co)
        {
            if (obj.TryGetComponent<Television>(out var tv))
            {
                tv.SetChannel(num);                
            }
        }
    }
}
