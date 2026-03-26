using System.Collections.Generic;
using UnityEngine;

public class VPLArg
{
    public string Name;
    public string Type;
}


public class VPLFunction : ScriptableObject
{
    // The arguments of this function and their type
    public virtual List<VPLArg> Args => new();

    public virtual void Execute(object[] input)
    {
        
    }
}
