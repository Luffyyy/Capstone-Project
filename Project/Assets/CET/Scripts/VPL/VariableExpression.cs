using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableExpression : BaseExpression
{
    public TMP_Dropdown VarField;

    public override void Activated(ExpressionTray tray)
    {
        base.Activated(tray);
        VarField.ClearOptions();
        VarField.AddOptions(Zone.GetVariableNames());
    }

    public override object Evaluate()
    {
        var name = VarField.itemText.text;
        if (name != null)
        {
            return Zone.GetVariable(VarField.options[VarField.value].text);
        }
        return null;
    }
}
