using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableBlock : BaseExpression
{
    public BaseVar Var;

    public override BlockNode SaveNode()
    {
        var node = new BlockNode()
        {
            DefinitionName = Defintion.name
        };

        Var.Save(node.Data);

        return node;
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
        Var.Load(node.Data);
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Var.Activated(zone);
    }

    public override object Evaluate()
    {
        return Var.GetValue();
    }
}
