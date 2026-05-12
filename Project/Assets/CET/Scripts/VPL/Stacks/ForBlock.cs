using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ForBlock : CBlock
{
    public BaseVar Var;
    public ExpressionTray FromExpr;
    public ExpressionTray ToExp;
    public ExpressionTray ByExp;

    public override BlockNode SaveNode()
    {
        BlockNode node = new()
        {
            DefinitionName = Defintion.name,
            Trays = new()
            {
                Tray.SaveNode()
            },
            ExpressionTrays = new()
            {
                FromExpr.SaveNode(),
                ToExp.SaveNode(),
                ByExp.SaveNode()
            }
        };

        Var.Save(node.Data);

        return node;
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        
        Tray.LoadNode(node.Trays[0]);
        FromExpr.LoadNode(node.ExpressionTrays[0]);
        ToExp.LoadNode(node.ExpressionTrays[1]);
        ByExp.LoadNode(node.ExpressionTrays[2]);
        Var.Load(node.Data);
    }

    public override void Activated(VPLZone zone)
    {
        FromExpr.Activated(zone);
        ToExp.Activated(zone);
        ByExp.Activated(zone);
        Var.Activated(zone, GetComponent<DraggableBlock>().IsNew);
        base.Activated(zone);
    }

    private int FromEval()
    {
        return (int)FromExpr.Evaluate();
    }

    private int ToEval()
    {
        return (int)ToExp.Evaluate();
    }

    private int ByEval()
    {
        return (int)ByExp.Evaluate();
    }

    private int VarValue()
    {
        return (int)Var.GetValue();
    }

    public override IEnumerator Execute()
    {
        // This essentially runs the blocks on top of a regular for loop
        // It starts with initializing our variable
        // Continues with a condition that runs each loop
        // And ends an increment rule

        // This block is largly inspired by Blockly's "count with" block
        
        // Automatically detect intention of the player
        // If the "from" is greater than the "to" then we assume they want to go in reverse
        int fromEval = FromEval();
        int byEval = Math.Abs(ByEval());
        bool reversed = fromEval > ToEval();
        if (reversed)
        {
            byEval = -byEval;
        }

        for (
            Var.SetValue(fromEval);
            reversed ? VarValue() >= ToEval() : VarValue() <= ToEval();
            Var.SetValue(VarValue() + byEval)
        )
        {
            yield return Tray.Execute();
        }
    }
}
