using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiteralBlock : BaseExpression
{
    public TMP_Dropdown TypeDropdown;

    public TMP_InputField ValueField;

    public TMP_Dropdown DropdownValueField;

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            Data = new()
            {
                new("Type", TypeDropdown.value.ToString()),
                new("InputValue", ValueField.text),
                new("DropdownValue", DropdownValueField.value.ToString()),
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
        var varValue = node.Data.Find(item => item.Key == "Type");
        if (int.TryParse(varValue.Value, out var typeInt))
        {
            TypeDropdown.value = typeInt;
        }
        varValue = node.Data.Find(item => item.Key == "InputValue");
        if (varValue.Value is string valueStr)
        {
            ValueField.text = valueStr;
        }
        varValue = node.Data.Find(item => item.Key == "DropdownValue");
        if (int.TryParse(varValue.Value, out var dropdownInt))
        {
            DropdownValueField.value = dropdownInt;
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        ValueField.interactable = true;
        DropdownValueField.interactable = true;
    }

    public void SetType(string type)
    {
        TypeDropdown.value = type switch
        {
            "num" => 0,
            "str" => 1,
            "bool" => 2,
            _ => 0
        };
    }

    public void OnTypeChanged()
    {
        var type = TypeDropdown.options[TypeDropdown.value].text;
        ValueField.gameObject.SetActive(true);
        DropdownValueField.gameObject.SetActive(false);

        if (type == "bool")
        {
            ValueField.gameObject.SetActive(false);
            DropdownValueField.gameObject.SetActive(true);
        } else if (type == "num")
        {
            ValueField.contentType = TMP_InputField.ContentType.DecimalNumber;
        } else if (type == "str")
        {
            ValueField.contentType = TMP_InputField.ContentType.Standard;
        }
    }

    public override object Evaluate()
    {
        var type = TypeDropdown.options[TypeDropdown.value].text;
        return type switch {
            "str" => ValueField.text,
            "num" => double.Parse(ValueField.text),
            "bool" => DropdownValueField.value == 1,
            _ => throw new System.Exception("Invalid type defined for VariableBlock: " + type)
        };
    }
}
