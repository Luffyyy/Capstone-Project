using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<BaseBlock>(out var block)) {
            block.OnDelete();
            block.transform.SetParent(transform); // Just to prevent it from returning it to the tray
            ExecuteEvents.Execute(block.gameObject, eventData, ExecuteEvents.endDragHandler);
            Destroy(block.gameObject);
        }
        gameObject.SetActive(false);
    }
}
