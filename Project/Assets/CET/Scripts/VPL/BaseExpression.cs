using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseExpression : MonoBehaviour
{
    [HideInInspector]
    public ExpressionTray Tray;
    [HideInInspector]
    public VPLZone Zone;

    public ValueConverter Converter;

    public virtual void Activated(ExpressionTray tray)
    {
        GetComponent<DraggableBlock>().IsNew = false;
        Tray = tray;
        Zone = tray.Zone;
    }

    public virtual object Evaluate()
    {
        return null;
    }
}
