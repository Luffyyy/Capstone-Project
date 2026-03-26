using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrintFunction", menuName = "VPL/Functions/Print")]
public class PrintFunction : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "msg", Type = "string" }
    };
    public override void Execute(params object[] input)
    {
        Debug.Log(input[0]);
    }
}
