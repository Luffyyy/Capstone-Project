using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FuncExpressionBlockDefinition", menuName = "VPL/Blocks/Function Expression")]
public class FuncExpressionBlockDefinition : FuncBlockDefinition
{
    void OnEnable()
    {
        PrefabName = "FuncExpressionBlock";
    }
}