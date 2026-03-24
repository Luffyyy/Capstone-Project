using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableBlock : BaseExpression
{
    public TMP_Dropdown VarField;

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
