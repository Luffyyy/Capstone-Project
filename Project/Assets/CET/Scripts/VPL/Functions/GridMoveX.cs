using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GridMoveX", menuName = "VPL/Functions/GridMoveX")]
public class GridMoveX : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "steps", Type = "num"},
    };
    public override IEnumerator ExecuteAsync(params object[] input)
    {
        var obj = Zone.ConnectedTo.ConnectedObjects[0];
        if (obj.TryGetComponent<GridMaze>(out var gridMaze))
        {
            var steps = int.Parse(input[0].ToString());
            yield return gridMaze.MovePlayer(Vector2Int.right * math.sign(steps), math.abs(steps));
        }
    }
}
