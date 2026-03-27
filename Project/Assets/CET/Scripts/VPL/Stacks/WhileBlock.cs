using System.Collections;
using System.Threading;
using UnityEngine;

public class WhileBlock : CBlock
{
    public ExpressionTray Expression;

    public override void SetName(string name)
    {
        
    }

    public override void SetColor(Color color)
    {
        
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
