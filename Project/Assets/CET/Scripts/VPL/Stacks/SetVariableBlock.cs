using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetVariableBlock : StackBlock
{
    public TMP_InputField VarField;

    public TMP_InputField ValueField;
    public TMP_Dropdown DropdownValueField;

    public string Type;

    public override void Awake()
    {
        VarField.interactable = false;
        if (ValueField != null)
        {
            ValueField.interactable = false;
        }
        if (DropdownValueField != null)
        {
            DropdownValueField.interactable = false;
        }
    }

    public override void Activated(VPLZone zone)
    {
        VarField.interactable = true;
        if (ValueField != null)
        {
            ValueField.interactable = true;
        }
        if (DropdownValueField != null)
        {
            DropdownValueField.interactable = true;
        }

        if (GetComponent<DraggableBlock>().IsNew)
        {
            VarField.text = zone.getVariableName();
        }

        base.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        // TODO: Define variables automatically before running so players can get variables recommended to them
        try
        {
            switch (Type)
            {
                case "string":
                    Zone.SetVariable(VarField.text, ValueField.text);
                    break;
                case "number":
                    Zone.SetVariable(VarField.text, float.Parse(ValueField.text));
                    break;
                case "bool":
                    Zone.SetVariable(VarField.text, DropdownValueField.value == 1);
                    break;
                default:
                    print("Invalid type defined for VariableBlock: " + Type);
                    break;
            }
        }
        catch (System.Exception e)
        {
            print(e);
            throw e; //TODO: handle exceptions
        }

        yield return null;
    }

}
