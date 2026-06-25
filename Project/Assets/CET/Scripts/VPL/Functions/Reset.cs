using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset", menuName = "VPL/Functions/Reset")]
public class Reset : FuncBlockDefinition
{
    public override void Execute(params object[] input)
    {
        var obj = Zone.ConnectedTo.ConnectedObjects[0];
        if (obj.TryGetComponent<GridMaze>(out var gridMaze))
        {
            gridMaze.ResetPlayer();
        }
        if (obj.TryGetComponent<PasswordDisplay>(out var display))
        {
            display.Reset();
        }
    }
}
