using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WhileBlock : CBlock
{
    public ExpressionTray Expression;

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            Trays = new()
            {
                Tray.SaveNode()
            },
            ExpressionTrays = new()
            {
                Expression.SaveNode()
            }
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        
        Tray.LoadNode(node.Trays[0]);
        Expression.LoadNode(node.ExpressionTrays[0]);
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        Expression.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        while (Expression != null && Helpers.VPLIsTrue(Expression.Evaluate()))
        {
            yield return Tray.Execute();
        }
    }
}
