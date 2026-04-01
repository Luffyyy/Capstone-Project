using Mirror;
using UnityEngine;

public class DevQuickServer : MonoBehaviour
{
    public NewNetworkDiscovery NetworkDiscovery;

    void Start()
    {
        NetworkManager.singleton.StartServer();
        NetworkDiscovery.AdvertiseServer();
    }
}
