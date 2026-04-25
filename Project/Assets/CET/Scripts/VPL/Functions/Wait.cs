using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "VPL/Functions/Wait")]
public class WaitFunction : VPLFunction
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "Seconds", Type = "num"},
    };

    public override IEnumerator ExecuteAsync(params object[] input)
    {
        yield return new WaitForSeconds((float)input[0]);
    }
}
