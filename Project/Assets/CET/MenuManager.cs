using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public NetworkDiscovery networkDiscovery;

    public GameObject serverList;
    public GameObject serverButton;

    public void Start()
    {
        networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
    }

    // Called to start hosting a game
    public void HostGame()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();

        gameObject.SetActive(false);
    }

    // Starts looking for games and renders it in a list
    public void LookForGames()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();

        UpdateServerList();
    }

    public void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);

        gameObject.SetActive(false);
    }

    public void UpdateServerList()
    {
        foreach (Transform child in serverList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var pair in discoveredServers)
        {
            var btn = Instantiate(serverButton, serverList.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().SetText(pair.Value.EndPoint.Address.ToString()); //TODO: lobby name instead of IP
            btn.GetComponent<Button>().onClick.AddListener(() => Connect(pair.Value)); // Connect on clicking the button
        }
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        Debug.Log($"Discovered Server: {info.serverId} | {info.EndPoint} | {info.uri}");

        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;

        UpdateServerList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
