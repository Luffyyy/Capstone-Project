using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomInPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomSpeed = 0.01f;
    public float minScale = 0.5f;
    public float maxScale = 3.0f;

    public static bool IsTouching = false;

    public Transform ContentTransform;

    public static float CurrentZoom = 1;

    public Slider Slider;

    public bool CanZoom()
    {
        return Input.touchCount > 1 || (Input.GetKey(KeyCode.LeftControl) && Input.mouseScrollDelta.y != 0);
    }

    void Awake()
    {
        if (ContentTransform == null)
        {
            ContentTransform = transform;
        }
    }

    public void OnZoomChanged(float zoom)
    {
        if (zoom != CurrentZoom)
        {
            CurrentZoom = zoom;
            ContentTransform.localScale = Vector3.one * CurrentZoom;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Application.isMobilePlatform)
        {
            IsTouching = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Application.isMobilePlatform)
        {
            IsTouching = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsTouching = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsTouching = CanZoom();
    }

    void Update()
    {
        print($"IsTouching={IsTouching}, CanZoom = {CanZoom()}");
        // Check if there are two touches on the device
        if (IsTouching && CanZoom())
        {

            if (Input.touchCount > 1)
            {
                ZoomViaTouch();
            } else
            {
                ZoomViaMouse();
            }
        }
    }

    void ZoomViaMouse()
    {
        Zoom(Input.mouseScrollDelta.y * zoomSpeed * 50);
    }

    void ZoomViaTouch()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // "Consume" the touch so other systems know it's handled
        // Note: Most standard Unity 'Drag' scripts check IsPointerOverGameObject
        EventSystem.current.IsPointerOverGameObject();

        // Find the position in the previous frame for each touch
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between frames
        float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

        Zoom(deltaMagnitudeDiff * zoomSpeed);
    }

    void Zoom(float increment)
    {
        CurrentZoom = Mathf.Clamp(CurrentZoom + increment, minScale, maxScale);
        Slider.value = CurrentZoom;
        ContentTransform.localScale = Vector3.one * CurrentZoom;
    }
}
