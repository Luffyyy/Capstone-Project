using System.Collections.Generic;
using UnityEngine;

public class VPLState : MonoBehaviour
{
    // Dictionary that holds variables of the VPL
    Dictionary<string, object> Variables;

    public List<BaseBlock> Blocks;

    public GameObject CurrentlySpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Execute()
    {
        foreach (var block in Blocks)
        {
            block.Execute();
        }
    }

    public void SetVariable(string str, object obj)
    {
        Variables[str] = obj;
    }

    public object GetVariable(string str)
    {
        return Variables[str];
    }

    public void Test()
    {
        print("Hi");
    }

    /**
        Spawns a block allowing the player to move it with their mouse until they chose where to place it
    */
    public void SpawnBlock(GameObject block)
    {
        print("Spawning");
        CurrentlySpawning = Instantiate(block, GameObject.Find("Menu").transform);
        CurrentlySpawning.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Input.mousePosition;
        if (CurrentlySpawning != null)
        {
            CurrentlySpawning.transform.position = pos;
        }
    }
}
