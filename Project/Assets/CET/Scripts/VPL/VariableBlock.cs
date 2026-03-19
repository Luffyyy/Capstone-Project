using TMPro;
using UnityEngine;

public class VariableBlock : BaseBlock
{
    public TMP_InputField VarField;

    // public TMP_Dropdown TypeField; For now assume number

    public TMP_InputField ValueField;

    public override void Execute()
    {
        // TODO: Define variables automatically before running so players can get variables recommended to them
        try
        {
            Zone.SetVariable(VarField.text, float.Parse(ValueField.text));
        }
        catch (System.Exception)
        {
            throw; //TODO: handle exceptions
        }
    }
}
