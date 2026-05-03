using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GridResetPlayer", menuName = "VPL/Functions/GridResetPlayer")]
public class GridResetPlayer : FuncBlockDefinition
{
    public override void Execute(params object[] input)
    {
        var obj = Zone.ConnectedTo.ConnectedObjects[0];
        if (obj.TryGetComponent<GridMaze>(out var gridMaze))
        {
            Debug.Log("reset");
            gridMaze.ResetPlayer();
        }
    }
}
