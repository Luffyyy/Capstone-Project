using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPLArg
{
    public string Name;
    public string Type;
}


public class VPLFunction : ScriptableObject
{
    public VPLZone Zone;
    // The arguments of this function and their type
    public virtual List<VPLArg> Args => new();

    public virtual object ExecuteWithReturn(object[] input)
    {
        return null; 
    }

    public virtual void Execute(object[] input)
    {
        
    }

    public virtual IEnumerator ExecuteAsync(object[] input)
    {
        yield return null;
    }
}
