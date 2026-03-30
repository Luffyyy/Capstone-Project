using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBlock : CBlock
{
    public BlockTray ElseTray;

    public ExpressionTray Expression;
    public ExpressionTray ElseExpression;

    public override BlockNode SaveNode()
    {
        List<BlockTrayNode> trays = new() { Tray.SaveNode() };
        List<ExpressionTrayNode> expressions = new() { Expression.SaveNode() };

        if (ElseTray != null)
        {
            trays.Add(ElseTray.SaveNode());
        }

        if (ElseExpression != null)
        {
            expressions.Add(ElseExpression.SaveNode());
        }

        return new()
        {
            DefinitionName = Defintion.name,
            Trays = trays,
            ExpressionTrays = expressions
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        Tray.LoadNode(node.Trays[0]);
        Expression.LoadNode(node.ExpressionTrays[0]);
        
        if (ElseTray != null)
        {
            ElseTray.LoadNode(node.Trays[1]);
        }
        if (ElseExpression != null)
        {
            ElseExpression.LoadNode(node.ExpressionTrays[1]);
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Expression.Activated(Zone);
        if (ElseExpression != null)
            ElseExpression.Activated(Zone);

        if (ElseTray != null)
        {
            ElseTray.Activated(zone);
        }
    }

    public override IEnumerator Execute()
    {
        if (Helpers.VPLIsTrue(Expression.Evaluate()))
        {
            yield return Tray.Execute();
        } else if (ElseTray != null)
        {
            if (ElseExpression != null)
            {
                if (!Helpers.VPLIsTrue(ElseExpression.Evaluate()))
                {
                    yield return null;
                }
            } 
            yield return ElseTray.Execute();
        }
    }
}
