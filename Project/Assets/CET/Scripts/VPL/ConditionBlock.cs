using UnityEngine;

public class ConditionBlock : BaseBlock
{
    public BlockTray IfTray;
    public BlockTray ElseTray;

    public ExpressionBlock Expression;
    public ExpressionBlock ElseExpression;

    public override void SetName(string name)
    {
        
    }

    public override void SetColor(Color color)
    {
        
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        IfTray.enabled = true;
        if (ElseTray != null)
        {
            ElseTray.enabled = true;
        }
    }

    public override void Execute()
    {
        if ((bool)Expression.Evaluate())
        {
            IfTray.Execute();
        } else if (ElseTray != null && ElseExpression != null && (bool)ElseExpression.Evaluate())
        {
            ElseTray.Execute();
        }
    }
}
