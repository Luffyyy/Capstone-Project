using UnityEngine;
using Mirror;

public class CameraSelector : MonoBehaviour
{
    public GameObject pcCamera;
    public GameObject mobileCamera;

    void Start()
    {
        if (NetworkServer.active)
        {
            // Server (host)
            pcCamera.SetActive(true);
            mobileCamera.SetActive(false);
        }
        else
        {
            // Clients
            pcCamera.SetActive(false);
            mobileCamera.SetActive(true);
        }
    }
}