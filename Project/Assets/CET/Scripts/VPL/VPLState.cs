using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

public class VPLState : MonoBehaviour
{
    // Dictionary that holds variables of the VPL
    Dictionary<string, object> Variables;

    public List<BaseBlock> Blocks = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        foreach (var block in Blocks)
        {
            block.state = this;
        }
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
}
