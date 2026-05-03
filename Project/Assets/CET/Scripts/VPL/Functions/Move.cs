using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "VPL/Functions/Move")]
public class Move : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "x", Type = "num"},
        new() { Name = "y", Type = "num"},
    };
    public override IEnumerator ExecuteAsync(params object[] input)
    {
        var obj = Zone.ConnectedTo.ConnectedObjects[0];
        if (obj.TryGetComponent<FloatingPlatform>(out var interactable))
        {
            yield return interactable.Move(new Vector3((float)input[0], 0, (float)input[1]));
        }
    }
}
