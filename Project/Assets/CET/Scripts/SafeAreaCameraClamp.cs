using UnityEngine;

public class SafeAreaCameraClamp : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        Rect safe = Screen.safeArea;
        Camera.main.rect = new Rect(
            safe.x / Screen.width,
            safe.y / Screen.height,
            safe.width / Screen.width,
            safe.height / Screen.height
        );
#endif
    }
}
