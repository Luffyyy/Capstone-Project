using TMPro;

public class LiteralBlock : BaseExpression
{
    public TMP_Dropdown TypeDropdown;

    public TMP_InputField ValueField;

    public TMP_Dropdown DropdownValueField;

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        ValueField.interactable = true;
        DropdownValueField.interactable = true;
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
        } else if (type == "number")
        {
            ValueField.contentType = TMP_InputField.ContentType.DecimalNumber;
        } else if (type == "string")
        {
            ValueField.contentType = TMP_InputField.ContentType.Standard;
        }
    }

    // Update is called once per frame
    public override object Evaluate()
    {
        var type = TypeDropdown.options[TypeDropdown.value].text;
        return type switch {
            "string" => ValueField.text,
            "number" => float.Parse(ValueField.text),
            "bool" => DropdownValueField.value == 1,
            _ => throw new System.Exception("Invalid type defined for VariableBlock: " + type)
        };
    }
}
