using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

// Improves the OnScreenStick by allowing it to "snap" to edges. This is often used in mobile games to make the controller much more comfortable to use
// This especially improves our use case of the phone as a controller
public class OnScreenStickSnap : OnScreenStick, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public new void OnDrag(PointerEventData eventData)
    {
        RectTransform parentRect = transform.parent as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform, 
            eventData.position, 
            null, 
            out Vector2 localPoint))
        {
            // Vector from the start position to the finger position, start position is the center in local position
            // Therefore we use that as the origin
            Vector2 centerOffset = parentRect.rect.center;
            Vector2 offset = localPoint - centerOffset;

            float distance = offset.magnitude;

            // If we are touching anywhere, even slightly...
            if (distance > 0)
            {
                // If we touch outside the range, snap to the edge
                if (distance > movementRange * 0.8f)
                {
                    offset = offset.normalized * movementRange;
                }
                else // Regular clamping so it doesn't leave the background
                {
                    offset = Vector2.ClampMagnitude(offset, movementRange);
                }
            }

            _rectTransform.anchoredPosition = offset;
            SendValueToControl(offset / movementRange);
        }
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        // Return to center
        _rectTransform.anchoredPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
    }
}