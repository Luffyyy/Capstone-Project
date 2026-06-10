using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public enum MoveDirection
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}

[CreateAssetMenu(fileName = "GridMove", menuName = "VPL/Functions/GridMove")]
public class GridMove : FuncBlockDefinition
{
    public MoveDirection Direction;
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
            var dirVec = Direction switch
            {
                MoveDirection.RIGHT => Vector2Int.right,
                MoveDirection.LEFT => Vector2Int.left,
                MoveDirection.UP => Vector2Int.down,
                MoveDirection.DOWN => Vector2Int.up,
                _ => throw new System.NotImplementedException(),
            };
            yield return gridMaze.MovePlayer(dirVec * math.sign(steps), math.abs(steps));
        }
    }
}
