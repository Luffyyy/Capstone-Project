using System.Collections;
using System.Collections.Generic;

public class ForEachBlock : CBlock
{
    public BaseVar Var;
    public ExpressionTray Expression;

    public override BlockNode SaveNode()
    {
        var node = new BlockNode()
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

        Var.Save(node.Data);

        return node;
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        
        Tray.LoadNode(node.Trays[0]);
        Var.Load(node.Data);

        Expression.LoadNode(node.ExpressionTrays[0]);
    }

    public override void Activated(VPLZone zone)
    {
        Expression.Activated(zone);
        Var.Activated(zone, GetComponent<DraggableBlock>().IsNew);
        base.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        var lst = (List<object>)Expression.Evaluate();
        foreach (var x in lst)
        {
            Var.SetValue(x);
            yield return Tray.Execute();
        }
    }
}
