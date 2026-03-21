using UnityEngine;

public class ForLoopBlock : CBlock
{
    public ExpressionTray Expression;

    void Awake()
    {
        Expression.Parent = this;
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
    }

    /**
        This is designed to work like 
    */
    private bool GetEval()
    {
        var eval = Expression.Evaluate();
        if (eval is bool v)
        {
            return v;
        } else
        {
            return eval != null;
        }
    }

    public override void Execute()
    {
        while (GetEval())
        {
            Tray.Execute();
        }
    }
}
