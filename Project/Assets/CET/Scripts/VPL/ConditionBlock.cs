using UnityEngine;

public class ConditionBlock : CBlock
{
    public BlockTray ElseTray;

    public ExpressionTray Expression;
    public ExpressionTray ElseExpression;

    void Awake()
    {
        Expression.Parent = this;
        if (ElseExpression != null)
            ElseExpression.Parent = this;
    }

    public override void SetName(string name)
    {
        
    }

    public override void SetColor(Color color)
    {
        
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        if (ElseTray != null)
        {
            ElseTray.enabled = true;
        }
    }

    public override void Execute()
    {
        var eval = Expression.Evaluate();
        if (eval is bool && (bool)eval)
        {
            Tray.Execute();
        } else if (ElseTray != null)
        {
            if (ElseExpression != null)
            {
                eval = ElseExpression.Evaluate();
                if (eval is bool && (bool)eval == false)
                {
                    return;
                }
            } 
            ElseTray.Execute();
        }
    }
}
