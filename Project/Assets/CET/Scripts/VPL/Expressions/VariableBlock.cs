using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableBlock : BaseExpression
{
    public BaseVar Var;

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            Data = new()
            {
                new("VarFieldValue", Var.Name)
            }
        };
    }

    public override void Awake()
    {
        base.Awake();
        IsExpression = true;
    }

    public override void SetName(string name)
    {
        
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        var varValue = node.Data.Find(item => item.Key == "VarFieldValue");
        if (varValue.Value is string varStr)
        {
            Var.SetName(varStr);
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Var.Activated(zone);
    }

    public override object Evaluate()
    {
        return Var.Evaluate();
    }
}
