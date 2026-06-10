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
        if (Input.touchCount > 1)
        {
            originalParent = transform.parent;
            eventData.pointerDrag = null;
            OnEndDrag(null);
            return;
        } // Avoid dragging while zooming

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
            transform.localScale = Vector2.one;
            
            // IMPORTANT: Allow raycasts to pass through this block 
            // so we can "see" the Tray/Blocks underneath it.
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.7f;

            if (!IsNew)
            {
                Zone.DeleteZone.Show();
            } else
            {
                var block = GetComponent<BaseBlock>();
                if (!string.IsNullOrEmpty(block.Defintion.Documentation))
                {
                    Zone.DocsZone.Show();
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (Input.touchCount > 1)
        {
            eventData.pointerDrag = null;
            OnEndDrag(null);
            return;
        }; // Avoid dragging while zooming

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
            if (!obj.TryGetComponent<BlockTray>(out var tray) && !obj.transform.parent.TryGetComponent(out tray))
            {
                // If all fails, Try main tray
                if (RectTransformUtility.RectangleContainsScreenPoint(Zone.MainTray.transform as RectTransform, Input.mousePosition))
                    tray = Zone.MainTray;
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
                transform.SetParent(originalParent, false);
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
                // It used to support multiple trays, but I decided to turn it off to make the game simpler
                // if (originalParent.childCount == 0 && originalParent.GetComponent<BlockTray>().IsRoot)
                // {
                //     Destroy(originalParent.gameObject);
                // }
            } else
            {
                originalParent.GetComponent<ExpressionTray>().RemoveCurrentExpression();
            }
        }

        if (Zone != null)
        {
            Zone.DeleteZone.Hide();
            Zone.DocsZone.Hide();
        }
    }
}