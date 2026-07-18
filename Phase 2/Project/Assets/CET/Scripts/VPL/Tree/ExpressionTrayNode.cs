using System;

[Serializable]
public class ExpressionTrayNode
{
    public ExpressionTrayNode()
    {
        Ident = Guid.NewGuid().ToString();
    }

    public string Ident;

    public BlockNode CurrentExpression;
}
