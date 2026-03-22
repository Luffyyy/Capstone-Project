using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class ExpressionTray : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [HideInInspector]
    public VPLZone Zone;
    public BaseExpression CurrentExpression;

    public bool IsActivated = false;

    public void Activated(VPLZone zone)
    {
        Zone = zone;
        IsActivated = true;
    }

    public object Evaluate()
    {
        return CurrentExpression.Evaluate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<BaseExpression>())
        {
            GetComponent<Outline>().enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<BaseExpression>())
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsActivated && eventData.pointerDrag.TryGetComponent<BaseExpression>(out var exp))
        {
            if (CurrentExpression != null)
            {
                Destroy(CurrentExpression.gameObject); // Only allow a single expression at a time
            }
            exp.transform.SetParent(transform);
            exp.Activated(this);
            GetComponent<Outline>().enabled = false;
            CurrentExpression = exp;
        }
    }
}
