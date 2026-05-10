using TMPro;
using UnityEngine;

public class BaseVar : MonoBehaviour
{
    VPLZone Zone;
    private string LastVariableName;

    public TMP_Dropdown VarField;

    public string Name => VarField.options[VarField.value].text;

    public void SetName(string name)
    {
        VarField.value = VarField.options.FindIndex(var => var.text == name);
    }

    public void Activated(VPLZone zone)
    {
        VarField.ClearOptions();
        VarField.AddOptions(Zone.GetVariableNames());
        zone.OnVariableNameChanged.AddListener(OnVariableNameChanged);
    }

    private void OnVariableNameChanged(string oldName, string newName)
    {
        string lookingFor = null;

        if (VarField.value < VarField.options.Count)
        {
            var opt = VarField.options[VarField.value];

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

    public object Evaluate()
    {
        return Zone.GetVariable(VarField.options[VarField.value].text);
    }
}
