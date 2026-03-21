using UnityEngine;

public class ForceClear : MonoBehaviour
{
    void OnPreRender()
    {
        GL.Clear(true, true, Color.black);
    }
}