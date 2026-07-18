using UnityEngine;
using UnityEngine.EventSystems;

public class DocsZone : DragZone
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<BaseBlock>(out var block)) {
            VPLMenu.Instance.ShowDocumentation(block.Defintion);
            ExecuteEvents.Execute(block.gameObject, eventData, ExecuteEvents.endDragHandler);
        }
        Hide();
    }
}
