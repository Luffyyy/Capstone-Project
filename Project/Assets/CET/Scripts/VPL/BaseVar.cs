using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseVar : MonoBehaviour
{
    VPLZone Zone;
    public string LastVariableName;
    public TMP_Dropdown VarField;

    private bool NoVariablesWereFound = true;

    public string Name {
        get {
            if (!IsVariable(VarField.value)) {
                return null;
            }

            return VarField.options[VarField.value]?.text;
        }
    }

    public bool IsExpression = false;

    public bool Test = false;

    public void Save(List<KeyValue> Data)
    {
        Data.Add(new KeyValue("VarName", Name));
    }

    public void Load(List<KeyValue> Data)
    {
        var varValue = Data.Find(item => item.Key == "VarName");
        if (varValue.Value is string varStr)
        {
            SetName(varStr);
        }
    }

    public void SetNameWithDefine(string name, bool silent=false)
    {
        if (VarField.options.FindIndex(var => var.text == name) == -1) // New name, add it
        {
            Test = true;
            Zone.UndefineVariable(LastVariableName, true);
            LastVariableName = name;
            Zone.DefineVariable(name);
            return; // RefreshOptions will deal with silenly updating this
        }
        SetName(name, silent);
    }

    public void SetName(string name, bool silent=false)
    {
        var index = VarField.options.FindIndex(var => var.text == name);
        
        if (index != -1)
        {
            VarField.SetValueWithoutNotify(index);
            if (!silent)
            {
                VarField.onValueChanged.Invoke(index);
            }
            VarField.RefreshShownValue();
        }
    }

    public bool IsVariable(int index)
    {
        if (NoVariablesWereFound)
        {
            return false; // No variables!
        } else
        {
            return index < VarField.options.Count-2;
        }
    }

    public void OnSelectedVarChanged(int index)
    {
        if (!IsVariable(index))
        {
            if (index != 0)
            {
                MenuManager.Instance.GetMenu<VPLMenu>("VPLMenu").ShowRenameVariableDialog(this, VarField.value == VarField.options.Count-1);
                if (!string.IsNullOrEmpty(LastVariableName))
                {
                    SetName(LastVariableName, true);
                }
            }
            return;
        }

        var name = VarField.options[index]?.text;
        if (name != LastVariableName)
        {
            var lastVariableName = LastVariableName; // We wanna be careful since calling define/undefine can trigger an event stack overflow
            LastVariableName = name;
            if (!string.IsNullOrEmpty(lastVariableName))
            {
                Zone.UndefineVariable(lastVariableName);
            }       
            Zone.DefineVariable(Name);
        }
    }

    public void SetValue(object value)
    {
        Zone.SetVariable(Name, value);
    }

    public object GetValue()
    {
        return Zone.GetVariable(Name);
    }

    public void Activated(VPLZone zone, bool isNew=false)
    {
        Zone = zone;
        string newName = null;
        if (IsExpression)
        {
            LastVariableName = null;
        }
        else if (isNew) {
            newName = zone.GetAndDefineVariableName(); // The variable is new, assign it a new random name
            LastVariableName = newName;
        }
        
        RefreshOptions();

        zone.OnVariableDefinitionChanged.AddListener(OnVariableDefinitionChanged);
        zone.OnVariableNameChanged.AddListener(OnVariableNameChanged);
    }

    private void OnVariableDefinitionChanged(string name, bool added)
    {
        if (added)
        {
            OnVariableNameChanged(null, name);
        } else
        {
            OnVariableNameChanged(name, null);
        }
    }

    private void OnVariableNameChanged(string oldName, string newName)
    {
        string lookingFor = LastVariableName;

        if (!string.IsNullOrEmpty(newName) && LastVariableName == oldName)
        {
            // If our variable changed, look for it. Otherwise, look for existing
            lookingFor = newName;
            LastVariableName = newName; // No need to define or undeifne when renaming variables
        }

        RefreshOptions();

        if (!string.IsNullOrEmpty(lookingFor))
        {
            SetName(lookingFor);
        }
    }

    void RefreshOptions()
    {
        VarField.ClearOptions();
        VarField.AddOptions(Zone.GetVariableNames());
        NoVariablesWereFound = VarField.options.Count == 0;

        if (NoVariablesWereFound)
        {
            VarField.AddOptions(new List<TMP_Dropdown.OptionData>
            {
               new("[Null]")
            });
            VarField.value = 0;
        }
        VarField.AddOptions(new List<TMP_Dropdown.OptionData>
        {
            new("Rename Variable"),
            new("New Variable"),
        });
        if (!string.IsNullOrEmpty(LastVariableName))
        {
            SetName(LastVariableName, true);
        } else if (!NoVariablesWereFound)
        {
            VarField.value = -1;
        }
    }

    void OnDestroy()
    {
        if (Zone != null)
        {
            // If an expression, react to changes and change the variable name of each use of said var
            Zone.OnVariableDefinitionChanged.RemoveListener(OnVariableDefinitionChanged);
            Zone.OnVariableNameChanged.RemoveListener(OnVariableNameChanged);
            if (!string.IsNullOrEmpty(LastVariableName))
            {
                Zone.UndefineVariable(LastVariableName);
            }
        }
    }
}