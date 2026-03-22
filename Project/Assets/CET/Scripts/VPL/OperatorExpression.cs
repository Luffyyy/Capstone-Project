using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperatorExpression : BaseExpression
{
    public TextMeshProUGUI NameText;
    public string Name;
    public ExpressionTray Exp1;
    public ExpressionTray Exp2;

    public bool IsUnary = false;

    public override void Activated(ExpressionTray tray)
    {
        base.Activated(tray);
        if (!IsUnary)
        {
            Exp1.Activated(Zone);
        }
        Exp2.Activated(Zone);
    }

    void Start()
    {
        NameText.text = Name;
        if (IsUnary)
        {
            Exp1.gameObject.SetActive(false);
        }
    }

    public override object Evaluate()
    {
        if (Converter != null)
        {
            if (IsUnary)
            {
                return Converter.Convert(Exp2.Evaluate());
            } else
            {
                return Converter.Convert(Exp1.Evaluate(), Exp2.Evaluate());
            }
        }
        return null;
    }
}
