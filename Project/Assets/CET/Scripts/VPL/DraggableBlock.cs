using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    [HideInInspector]
    public bool IsFake = false;
    [HideInInspector]
    public bool IsNew = false;

    private BlockTray lastTray;

    public bool IsStackBlock;

    public VPLZone Zone => GetComponent<BaseBlock>().Zone;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        IsStackBlock = !GetComponent<BaseBlock>().IsExpression;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (IsFake)
        {
            var instance = Instantiate(this, GameObject.Find("Menu").transform).gameObject;
            var dobj = instance.GetComponent<DraggableBlock>();
            dobj.IsNew = true;
            dobj.IsFake = false;
            instance.GetComponent<RectTransform>().position = eventData.position;
            eventData.pointerDrag = instance; // Pass the pointer drag event ot the copy

            // Trigger the BeingDrag logic manually
            ExecuteEvents.Execute(instance, eventData, ExecuteEvents.beginDragHandler);
        } else
        {
            if (!IsNew)
            {
                originalParent = transform.parent;
            }
            
            // Move to the root canvas so it renders on top of everything
            transform.SetParent(canvas.transform);
            
            // IMPORTANT: Allow raycasts to pass through this block 
            // so we can "see" the Tray/Blocks underneath it.
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.7f;

            if (!IsNew)
            {
                Zone.DeleteZone.SetActive(true); //TODO: animate it
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // 1. Move the block
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // 2. Check what is under the mouse
        // eventData.pointerEnter is the object currently under the cursor
        var obj = eventData.pointerEnter;

        if (!IsStackBlock)
        {
            return;
        }

        if (obj != null)
        {
            var tray = obj.GetComponent<BlockTray>();
            if (tray == null)
            {
                tray = obj.transform.parent.GetComponent<BlockTray>();
            }
            if (tray != null)
            {
                // Tell the tray where the mouse is so it can move the ghost
                tray.UpdateGhostPosition(gameObject, eventData.position);
                if (tray != lastTray)
                {
                    if (lastTray != null) lastTray.DestroyPreview();
                    lastTray = tray;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // If we didn't land in a tray, return to original parent
        // In case it was a new block, delete it

        if (transform.parent == canvas.transform) {
            if (IsNew)
            {
                Destroy(gameObject);
            } else
            {
                transform.SetParent(originalParent);
            }
        }

        if (lastTray != null)
        {
            lastTray.DestroyPreview();
            lastTray = null;
        }

        if (originalParent != null && originalParent != transform.parent)
        {
            if (IsStackBlock)
            {
                if (originalParent.childCount == 0 && originalParent.GetComponent<BlockTray>().IsRoot)
                {
                    Destroy(originalParent.gameObject);
                }
            } else
            {
                originalParent.GetComponent<ExpressionTray>().RemoveCurrentExpression();
            }
        }

        if (Zone != null)
        {
            Zone.DeleteZone.SetActive(false);
        }
    }
}