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
            ElseTray.enabled = true;
        }
    }

    public override void Execute()
    {
        if (Helpers.VPLIsTrue(Expression.Evaluate()))
        {
            Tray.Execute();
        } else if (ElseTray != null)
        {
            if (ElseExpression != null)
            {
                if (!Helpers.VPLIsTrue(ElseExpression.Evaluate()))
                {
                    return;
                }
            } 
            ElseTray.Execute();
        }
    }
}
