using Mirror;
using UnityEngine;

public class SceneCamera : NetworkBehaviour
{
    void Awake()
    {
        RegisterCamera();

        GameObject.Find("MainCamera").GetComponent<CameraTransition>().target = transform;
    }

    void RegisterCamera()
    {
        NewNetworkManager.singleton.SceneCameras.Add(this);
        GetComponent<Camera>().enabled = false; // Disable self so we can do camera transition
    }
}
