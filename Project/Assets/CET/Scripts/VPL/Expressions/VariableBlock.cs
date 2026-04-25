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

    public override void Awake()
    {
        base.Awake();
        IsExpression = true;
    }

    public override void SetName(string name)
    {
        
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
        zone.OnVariableNameChanged.AddListener(OnVariableNameChanged);
    }

    private void OnVariableNameChanged(string oldName, string newName)
    {
        var opt = VarField.options[VarField.value];
        string lookingFor = null;

        if (opt != null)
        {
            var varName = opt.text;
            lookingFor = varName;

            if (varName == oldName) // Our var name was changed
            {
                lookingFor = newName;
            } // else our var name was not changed, look for it
        }

        VarField.ClearOptions();
        VarField.AddOptions(Zone.GetVariableNames());

        if (lookingFor != null)
        {
            VarField.value = VarField.options.FindIndex(varName => varName.text == lookingFor);
        }
    }

    void OnDestroy()
    {
        if (Zone != null)
        {
            Zone.OnVariableNameChanged.RemoveListener(OnVariableNameChanged);
        }
    }

    public override object Evaluate()
    {
        return Zone.GetVariable(VarField.options[VarField.value].text);
    }
}
