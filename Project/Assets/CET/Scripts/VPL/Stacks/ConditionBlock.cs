using System.Collections;
using UnityEngine;

public class ConditionBlock : CBlock
{
    public BlockTray ElseTray;

    public ExpressionTray Expression;
    public ExpressionTray ElseExpression;

    public override void SetName(string name)
    {
        
    }

    public override void SetColor(Color color)
    {
        
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
