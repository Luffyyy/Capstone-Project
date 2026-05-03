using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Decrypt", menuName = "VPL/Functions/Decrypt")]
public class DecryptFunction : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "char", Type = "str" },
    };

    public override object ExecuteWithReturn(params object[] input)
    {
        char c = ((string)input[0])[0];
        if (c < 'a' || c > 'z') return null; // Ignore characters that aren't between a-z

        c -= 'a';

        Debug.Log((c-6) % 26);

        return ((char)((c-6) % 26 + 'a')).ToString();
    }
}
