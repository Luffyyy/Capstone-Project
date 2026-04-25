using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseExpression : BaseBlock
{
    public override void Awake()
    {
        base.Awake();
        hasTopPort = false;
        hasBottomPort = false;
        IsExpression = true;
    }
}
