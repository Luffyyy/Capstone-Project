using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetVariableBlock : StackBlock
{
    public TMP_InputField VarField;

    public ExpressionTray Tray;

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            Data = new()
            {
                new("VarFieldValue", VarField.text)
            },
            ExpressionTrays = new()
            {
                Tray.SaveNode()
            }
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        
        var varValue = node.Data.Find(item => item.Key == "VarFieldValue");
        if (varValue.Value is string varStr)
        {
            VarField.text = varStr;
        }

        Tray.LoadNode(node.ExpressionTrays[0]);
    }

    public override void Activated(VPLZone zone)
    {
        VarField.interactable = true;

        if (GetComponent<DraggableBlock>().IsNew)
        {
            VarField.text = zone.getVariableName();
        }

        if (Tray != null)
        {
            Tray.Activated(zone);
        }

        base.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        Zone.SetVariable(VarField.text, Tray.Evaluate());

        yield return null;
    }

}
