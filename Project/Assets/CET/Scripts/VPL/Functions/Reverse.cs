using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reverse", menuName = "VPL/Functions/Reverse")]
public class ReverseFunction : FuncExpressionBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "", Type = "str"},
    };
    public override object ExecuteWithReturn(params object[] input)
    {
        var s = input[0].ToString();
        var newS = "";

        for (int i=s.Length-1; i>=0; i--)
        {
            newS += s[i];
        }

        return newS;
    }
}
