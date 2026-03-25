using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class ExpressionTray : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [HideInInspector]
    public VPLZone Zone;
    public BaseExpression CurrentExpression;

    public BaseExpression DefaultBlock;

    public bool IsActivated = false;

    void Awake()
    {
        if (CurrentExpression == null && DefaultBlock != null)
        {
            CurrentExpression = DefaultBlock;
            CurrentExpression.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void Activated(VPLZone zone)
    {
        Zone = zone;
        IsActivated = true;
        if (CurrentExpression != null)
        {
            CurrentExpression.Activated(zone);
        }
    }

    public object Evaluate()
    {
        if (CurrentExpression != null)
        {
            return CurrentExpression.Evaluate();
        } else
        {
            return null;            
        }
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
            if (CurrentExpression != null && CurrentExpression != exp)
            {
                Destroy(CurrentExpression.gameObject); // Only allow a single expression at a time
            }
            exp.transform.SetParent(transform);
            exp.Activated(Zone);
            GetComponent<Outline>().enabled = false;
            CurrentExpression = exp;
        }
    }
}
