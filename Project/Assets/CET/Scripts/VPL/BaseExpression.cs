using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseExpression : BaseBlock
{
    public ValueConverter Converter;

    public virtual object Evaluate()
    {
        return null;
    }
}
