using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OperatorBlock : BaseBlock
{
    public ExpressionTray Exp1;
    public ExpressionTray Exp2;

    public TextMeshProUGUI Operator;

    public ValueConverter Converter;

    public bool IsUnary = false;

    void Start()
    {
        if (IsUnary)
        {
            Exp1.gameObject.SetActive(false);
        }
    }

    public override void SetName(string name)
    {
        
    }

    public override void Awake()
    {
        base.Awake();
        IsExpression = true;
    }

    public override BlockNode SaveNode()
    {
        return new()
        {
            DefinitionName = Defintion.name,
            ExpressionTrays = new()
            {
                Exp1.SaveNode(),
                Exp2.SaveNode(),
            }
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        Exp1.LoadNode(node.ExpressionTrays[0]);
        Exp2.LoadNode(node.ExpressionTrays[1]);
    }

    public override void SetDefinition(BlockDefinition def)
    {
        base.SetDefinition(def);

        if (def != null && def is OperatorDefinition op)
        {
            Converter = op.Converter;
            Operator.SetText(op.Sign);
            IsUnary = op.IsUnary;
            Exp1.gameObject.SetActive(!IsUnary);
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        if (!IsUnary)
        {
            Exp1.Activated(Zone);
        }
        Exp2.Activated(Zone);
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
