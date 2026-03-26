using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FuncBlockDefinition", menuName = "VPL/Blocks/Function")]
public class FuncBlockDefinition : BlockDefinition
{
    // The function that this block will run
    public VPLFunction Function;

    public override string PrefabName => "FuncBlock";
}