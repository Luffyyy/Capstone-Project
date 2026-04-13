using Mirror;
using UnityEngine;

public class SceneCamera : NetworkBehaviour
{
    void Awake()
    {
        RegisterCamera();

        if (gameObject.scene.path == NewNetworkManager.singleton.CurrentLevel)
        {
            GameObject.Find("MainCamera").GetComponent<CameraTransition>().target = transform;
        }
    }

    void RegisterCamera()
    {
        NewNetworkManager.singleton.SceneCameras.Add(this);
        GetComponent<Camera>().enabled = false; // Disable self so we can do camera transition
    }
}
