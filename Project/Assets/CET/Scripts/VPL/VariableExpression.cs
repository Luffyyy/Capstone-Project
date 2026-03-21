using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableExpression : BaseExpression
{
    public TMP_Dropdown VarField;

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        VarField.ClearOptions();
        VarField.AddOptions(zone.GetVariableNames());
    }

    public override object Evaluate()
    {
        var name = VarField.itemText.text;
        if (name != null)
        {
            return Zone.GetVariable(VarField.options[VarField.value].text);
        } else
        {
            return null;
        }
    }
}
