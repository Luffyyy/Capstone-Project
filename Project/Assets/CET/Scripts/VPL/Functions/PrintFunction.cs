using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrintFunction", menuName = "VPL/Functions/Print")]
public class PrintFunction : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "msg", Type = "str" }
    };
    public override void Execute(params object[] input)
    {
        Zone.PrintToConsole(input[0].ToString());
    }
}
