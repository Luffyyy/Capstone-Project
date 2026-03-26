using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetVariableBlock : StackBlock
{
    public TMP_InputField VarField;

    public ExpressionTray Tray;

    public LiteralBlock LiteralBlock;

    public string Type;

    public override void Activated(VPLZone zone)
    {
        VarField.interactable = true;

        if (GetComponent<DraggableBlock>().IsNew)
        {
            VarField.text = zone.getVariableName();
        }

        if (Tray != null)
        {
            Tray.Activated(zone);
        }

        base.Activated(zone);
    }

    public override IEnumerator Execute()
    {
        Zone.SetVariable(VarField.text, Tray.Evaluate());

        yield return null;
    }

}
