using UnityEngine;
using UnityEngine.EventSystems;

public class VPLDropZone : MonoBehaviour, IDropHandler
{
    public VPLZone Zone;
    public void OnDrop(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null && obj.TryGetComponent<DraggableBlock>(out var block))
        {
            if (!block.IsStackBlock) return;

            var tray = Zone.CreateTray();
            obj.transform.SetParent(tray.transform);
            block.GetComponent<BaseBlock>().Activated(Zone);
        }
    }
}
