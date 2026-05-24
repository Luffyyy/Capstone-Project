using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class MazeCellData
{
    public bool Visited = false;
    public bool IsWall = true;
    public int X,Y;
}

public class GridMaze : Interactable
{
    public int Size = 4;
    [HideInInspector, SyncVar(hook=nameof(OnFlatDataReceived))]
    public MazeCellData[] FlatData;
    public MazeCellData[,] Data;
    Stack<Vector2Int> pathStack = new();

    MazeMenu menu;

    [SyncVar(hook=nameof(UpdatePlayer)), HideInInspector]
    public Vector2Int Player;

    public UnityEvent OnReachedGoal;

    readonly Vector2Int[] dirs =
    {
        Vector2Int.left * 2,  
        Vector2Int.right * 2,  
        Vector2Int.up * 2,  
        Vector2Int.down * 2,  
    };

    public void OnDataChanged()
    {
        if (menu != null)
        {
            menu.BuildGrid();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Generate();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        menu = (MazeMenu)MenuManager.Instance.AddMenu("MazeMenu");
        menu.Maze = this;
        OnDataChanged();
    }

    [TargetRpc]
    public override void TargetInteract(NetworkConnectionToClient target)
    {
        base.TargetInteract(target);
        MenuManager.Instance.OpenMenu("MazeMenu");
    }

    public void Generate()
    {
        Data = new MazeCellData[Size, Size];
        for (int j=0; j<Size; j++)
        {
            for (int i=0; i<Size; i++)
            {
                Data[i,j] = new MazeCellData();
            }
        }

        GeneratePath();
        FlatData = Helpers.Flatten2DArray(Data);
    }

    public void OnFlatDataReceived(MazeCellData[] oldData, MazeCellData[] newData)
    {
        Data = Helpers.Unflatten2DArray(newData, Size, Size);
        OnDataChanged();
    }
    
    public bool IsCoordValid(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < Size && coord.y >= 0 && coord.y < Size;
    }

    public void UpdatePlayer(Vector2Int oldPlayer, Vector2Int newPlayer)
    {
        menu.UpdateGrid();
    }

    public void ResetPlayer()
    {
        Player = Vector2Int.zero;
        if (isServer)
        {
            menu.UpdateGrid();
        }
    }

    public IEnumerator MovePlayer(Vector2Int dir, int steps)
    {
        for (int i=1; i<=steps; i++)
        {
            var nextStep = Player + dir;

            if (!IsCoordValid(nextStep) || Data[nextStep.x, nextStep.y].IsWall) {
                ResetPlayer();
                break;
            }

            Player = nextStep;
            if (isServer)
            {
                menu.UpdateGrid();
            }
            yield return new WaitForSeconds(0.25f);
        }
        if (Player == new Vector2Int(Size-1, Size-1))
        {
            OnReachedGoal.Invoke();
        }
    }

    public void GeneratePath()
    {
        var curr = Vector2Int.zero;
        Data[curr.x, curr.y].IsWall = false;
        Data[Size-1, Size-1].IsWall = false;
        Data[curr.x, curr.y].Visited = true;
        pathStack.Push(curr);

        while (pathStack.Count > 0)
        {
            List<Vector2Int> neighbors = GetValidNeighbors(curr);

            if (neighbors.Count > 0)
            {
                var next = neighbors[Random.Range(0, neighbors.Count)];

                var coordInbetween = curr + (next - curr)/2;
                var cellInBetween = Data[coordInbetween.x, coordInbetween.y];
                cellInBetween.Visited = true;
                cellInBetween.IsWall = false;

                var cell = Data[curr.x, curr.y];
                cell.Visited = true;
                cell.IsWall = false;
                curr = next;
                pathStack.Push(curr);
            } else if (pathStack.Count > 0) // Backtrack, we hit a deadend
            {
                curr = pathStack.Pop();
            }
        }
    }

    public List<Vector2Int> GetValidNeighbors(Vector2Int curr)
    {
        var list = new List<Vector2Int>();

        foreach (var dir in dirs)
        {
            var coord = curr + dir;
            
            if (IsCoordValid(coord) && !Data[coord.x, coord.y].Visited)
            {
                list.Add(coord);
            }
        }

        return list;
    }
}
