using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MazeMenu : MenuBase
{
    public GameObject Cell;

    [HideInInspector]
    public GridMaze Maze;

    private Transform content;

    private RectTransform blackboard;

    GameObject[,] cells;

    void Awake()
    {
        blackboard = (RectTransform)transform.Find("BlackBoard");
        content = blackboard.GetChild(0);
    }


    public override void Show()
    {
        base.Show();

        if (NetworkClient.localPlayer != null)
            NetworkClient.localPlayer.GetComponent<PlayerController>().CmdSetFocusingOnPhone(true);
    }

    public override void Hide()
    {
        base.Hide();

        if (NetworkClient.localPlayer != null)
            NetworkClient.localPlayer.GetComponent<PlayerController>().CmdSetFocusingOnPhone(false);
    }

    public void UpdateGrid()
    {
        for (int j=0; j<Maze.Size; j++)
        {
            for (int i=0; i<Maze.Size; i++)
            {
                var pos = new Vector2Int(i,j);

                Color col = Color.white;

                if (pos == Maze.Player)
                {
                    col = Color.blue;
                } else if (pos == Maze.Goal)
                {
                    col = Color.green;
                } else if (Maze.Data[i,j].IsWall)
                {
                    col = Color.black;
                }
                cells[i,j].GetComponent<Image>().color = col;
            }
        }

    }

    public void BuildGrid()
    {
        var height = blackboard.sizeDelta[1];
        var size = Maze.Size;
        cells = new GameObject[size, size];
        content.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (height / (size+2) - 4);
        for(int i=0; i<content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        for (int j=0; j<Maze.Size; j++)
        {
            for (int i=0; i<Maze.Size; i++)
            {
                cells[i, j] = Instantiate(Cell, content);
                cells[i, j].name = $"cell-{i},{j}";
            }
        }
        UpdateGrid();
    }
}