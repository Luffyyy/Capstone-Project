using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuncBlock : BaseBlock
{
    public List<ExpressionTray> Trays;

    public GameObject ArgNameObject;
    public GameObject ExpressionTrayObject;
    public GameObject LiteralBlockObject;

    public FuncBlockDefinition FunctionDef => Defintion as FuncBlockDefinition;

    public override BlockNode SaveNode()
    {
        List<ExpressionTrayNode> expressions = new();

        foreach (var tray in Trays)
        {
            expressions.Add(tray.SaveNode());
        }
        
        return new()
        {
            DefinitionName = Defintion.name,
            ExpressionTrays = expressions
        };
    }

    public override void LoadNode(BlockNode node)
    {
        base.LoadNode(node);
        
        for (int i=0; i< Trays.Count; i++)
        {
            var tray = Trays[i];
            tray.Activated(Zone);
            tray.LoadNode(node.ExpressionTrays[i]);
        }
    }

    public override void SetDefinition(BlockDefinition def)
    {
        base.SetDefinition(def);

        if (def != null && def is FuncBlockDefinition fb)
        {
            fb.Zone = Zone;
            foreach (var arg in fb.Args)
            {
                var name = Instantiate(ArgNameObject, transform);
                var text = name.GetComponent<TextMeshProUGUI>();
                text.SetText(arg.Name);
                text.fontSize = IsExpression ? 28 : 36;

                var tray = Instantiate(ExpressionTrayObject, transform);
                var trayComp = tray.GetComponent<ExpressionTray>();
                var exp = Instantiate(LiteralBlockObject, tray.transform).GetComponent<LiteralBlock>();
                exp.SetType(arg.Type);
                trayComp.DefaultBlock = exp;
                exp.gameObject.SetActive(false);
                trayComp.CurrentExpression = exp;
                Trays.Add(trayComp);
            }
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);

        if (FunctionDef != null)
        {
            FunctionDef.Zone = zone;
        }

        foreach (var tray in Trays)
        {
            tray.Activated(zone);
        }
    }

    public override IEnumerator Execute()
    {
        var args = new object[Trays.Count];
        for (int i=0; i < Trays.Count; i++)
        {
            args[i] = Trays[i].Evaluate();
        }

        if ((Defintion as FuncBlockDefinition).IsAsync)
        {
            yield return FunctionDef.ExecuteAsync(args);
        } else
        {
            FunctionDef.Execute(args);
        }

        yield return null;
    }

    // Special case: it can also be an expression in some cases
    public override object Evaluate()
    {
        var args = new object[Trays.Count];
        for (int i=0; i < Trays.Count; i++)
        {
            args[i] = Trays[i].Evaluate();
        }
        return FunctionDef.ExecuteWithReturn(args);
    }
}
