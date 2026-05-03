using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DecryptFunction", menuName = "VPL/Functions/Decrypt")]
public class DecryptFunction : FuncExpressionBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "char", Type = "str" },
        new() { Name = "key", Type = "num" },
    };

    public override object ExecuteWithReturn(params object[] input)
    {
        char c = ((string)input[0])[0];
        int k = int.Parse(input[1].ToString());
        if (c < 'a' || c > 'z') return null; // Ignore characters that aren't between a-z

        c -= 'a';

        return ((char)((c-k) % 26 + 'a')).ToString();
    }
}
