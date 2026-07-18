using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VPLArg
{
    public string Name;
    public string Type;
}

[CreateAssetMenu(fileName = "FuncBlockDefinition", menuName = "VPL/Blocks/Function")]
public class FuncBlockDefinition : BlockDefinition
{
    // The arguments of this function and their type
    public virtual List<VPLArg> Args => new();

    public bool IsAsync = false;

    [HideInInspector]
    public VPLZone Zone;

    void OnEnable()
    {
        PrefabName = "FuncBlock";
    }

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