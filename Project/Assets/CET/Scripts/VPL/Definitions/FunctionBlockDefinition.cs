using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FunctionBlockDefinition", menuName = "VPL/Blocks/Function")]
public class FunctionBlockDefinition : BlockDefinition
{
    // The function that this block will run
    public VPLFunction Function;

    // The arguments of this function and their type
    public Dictionary<string, string> Args;

    public override string PrefabName => "FunctionBlock";
}