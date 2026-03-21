using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class ExpressionTray : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    public BaseBlock Parent;
    public BaseExpression CurrentExpression;

    public object Evaluate()
    {
        return CurrentExpression.Evaluate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<BaseExpression>())
        {
            GetComponent<Outline>().enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<BaseExpression>())
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<BaseExpression>(out var exp))
        {
            exp.transform.SetParent(transform);
            exp.Activated(Parent.Zone);
            GetComponent<Outline>().enabled = false;
            CurrentExpression = exp;
        }
    }
}
