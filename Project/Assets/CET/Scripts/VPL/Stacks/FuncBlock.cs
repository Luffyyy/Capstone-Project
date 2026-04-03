using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuncBlock : StackBlock
{
    public List<ExpressionTray> Trays;

    public GameObject ArgNameObject;
    public GameObject ExpressionTrayObject;
    public GameObject LiteralBlockObject;

    public VPLFunction Func;

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
            Func = fb.Function;
            Func.Zone = Zone;
            foreach (var arg in fb.Function.Args)
            {
                var name = Instantiate(ArgNameObject, transform);
                name.GetComponent<TextMeshProUGUI>().SetText(arg.Name);

                var tray = Instantiate(ExpressionTrayObject, transform);
                var trayComp = tray.GetComponent<ExpressionTray>();
                var exp = Instantiate(LiteralBlockObject, tray.transform).GetComponent<LiteralBlock>();
                exp.SetType(arg.Type);
                trayComp.DefaultBlock = exp;
                trayComp.CurrentExpression = exp;
                Trays.Add(trayComp);
            }
        }
    }

    public override void Activated(VPLZone zone)
    {
        base.Activated(zone);
        if (Func != null)
        {
            Func.Zone = zone;
        }
        foreach (var tray in Trays)
        {
            tray.Activated(zone);
        }
    }

    // Update is called once per frame
    public override IEnumerator Execute()
    {
        var args = new object[Trays.Count];
        for (int i=0; i < Trays.Count; i++)
        {
            args[i] = Trays[i].Evaluate();
        }
        Func.Execute(args);

        yield return null;
    }
}
