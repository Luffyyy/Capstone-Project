using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VPLMenu : MenuBase
{
    public Dictionary<int, VPLZone> Zones = new();

    private VPLZone LastOpenZone;

    public Dialog RenameVariableDialog;

    public TMP_InputField RenameVariableName;

    private bool renameVariableIsDefining;

    private BaseVar editingVar;

    public VPLZone VPLEditMenuPrefab;

    public Dialog DocumentationDialog;

    public TextMeshProUGUI DocumentationText;

    public static VPLMenu Instance;

    void Start()
    {
        Instance = this;

#if UNITY_EDITOR
        var go = Instantiate(VPLEditMenuPrefab, transform);
        go.EditorMode = true;
        go.Activated();
        AddZone(go, 123456);
#endif
    }

    public void AddZone(VPLZone zone, int id)
    {
        Zones[id] = zone;
        zone.Hide();
    }

    public void OpenVPLZone(int id)
    {
        if (LastOpenZone != null)
        {
            LastOpenZone.Hide();
            LastOpenZone = null;
        }

        LastOpenZone = Zones[id];
        LastOpenZone.Show();
    }

    public void ShowDocumentation(BlockDefinition block)
    {
        DocumentationText.text = $"<u>{block.Name}</u>\n{block.Documentation}";
        DocumentationDialog.Show();
    }

    public void DoRenameVariable()
    {
        if (renameVariableIsDefining)
        {
            editingVar.SetNameWithDefine(RenameVariableName.text);
        } else
        {
            LastOpenZone.SetVariableName(editingVar.LastVariableName, RenameVariableName.text);
        }
        RenameVariableDialog.Hide();
    }

    public void ShowRenameVariableDialog(BaseVar v, bool isDefiningNewVar)
    {
        editingVar = v;
        renameVariableIsDefining = isDefiningNewVar;

        if (isDefiningNewVar)
        {
            RenameVariableName.text = LastOpenZone.GetVariableName();
            RenameVariableDialog.SetTitle("Define a New Variable");
        } else
        {
            RenameVariableName.text = v.LastVariableName;
            RenameVariableDialog.SetTitle("Rename Existing Variable");
        }
        RenameVariableDialog.Show();
    }
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MenuManager.Instance.OpenMenu("VPLMenu");
            OpenVPLZone(123456);
        }
    }
#endif
}
