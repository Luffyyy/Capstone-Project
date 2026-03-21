using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseExpression : MonoBehaviour
{
    public VPLZone Zone;
    public virtual void Activated(VPLZone zone)
    {
        GetComponent<DraggableBlock>().IsNew = false;
        Zone = zone;
    }

    public virtual object Evaluate()
    {
        return null;
    }
}
