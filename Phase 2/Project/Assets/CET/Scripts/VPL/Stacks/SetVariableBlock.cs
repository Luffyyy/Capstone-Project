using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetVariableBlock : BaseBlock
{
    public ExpressionTray Tray;

    public BaseVar Var;

    public override BlockNode SaveNode()
    {
        var node = new BlockNode()
        {
            DefinitionName = Defintion.name,
            ExpressionTrays = new()
            {
                Tray.SaveNode()
            }
        };

        Var.Save(node.Data);

        return node;
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        Var.Load(node.Data);
        Tray.LoadNode(node.ExpressionTrays[0]);
    }

    public override void Activated(VPLZone zone)
    {
        Var.Activated(zone, GetComponent<DraggableBlock>().IsNew);

        if (Tray != null)
        {
            Tray.Activated(zone);
        }

        base.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        Var.SetValue(Tray.Evaluate());

        yield return null;
    }
}