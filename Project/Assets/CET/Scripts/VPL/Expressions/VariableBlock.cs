using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableBlock : BaseExpression
{
    public TMP_Dropdown VarField;

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            Data = new()
            {
                new("VarFieldValue", VarField.value.ToString())
            }
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        var varValue = node.Data.Find(item => item.Key == "VarFieldValue");
        if (int.TryParse(varValue.Value, out var varInt))
        {
            VarField.value = varInt;
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        VarField.ClearOptions();
        VarField.AddOptions(Zone.GetVariableNames());
    }

    public override object Evaluate()
    {
        return Zone.GetVariable(VarField.options[VarField.value].text);
    }
}
