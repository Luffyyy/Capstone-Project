using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "VPL/Functions/Wait")]
public class WaitFunction : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "seconds", Type = "num"},
    };

    public override IEnumerator ExecuteAsync(params object[] input)
    {
        yield return new WaitForSeconds((float)input[0]);
    }
}
