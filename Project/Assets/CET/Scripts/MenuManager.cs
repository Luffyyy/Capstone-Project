using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    readonly Dictionary<long, DiscoveryResponse> discoveredServers = new Dictionary<long, DiscoveryResponse>();

    public NewNetworkDiscovery networkDiscovery;

    public GameObject serverList;

    public Dialog serversDialog;
    public Dialog hostGameDialog;

    public TMP_InputField ServerName;

    public GameObject serverButton;

    [HideInInspector] public string ServerNameStr;

    public static MenuManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
    }

    public void OpenHostGameDialog()
    {
        hostGameDialog.Show();
    }

    // Called to start hosting a game
    public void HostGame()
    {
        ServerNameStr = ServerName.text;
        discoveredServers.Clear();
        NetworkManager.singleton.StartServer();

        networkDiscovery.AdvertiseServer();

        gameObject.SetActive(false);
    }

    // Starts looking for games and renders it in a list
    public void LookForGames()
    {
        serversDialog.Show();
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();

        UpdateServerList();
    }

    public void Connect(DiscoveryResponse info)
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
            btn.GetComponentInChildren<TextMeshProUGUI>().SetText(pair.Value.Name + " (0 Players)");
            btn.GetComponent<Button>().onClick.AddListener(() => Connect(pair.Value)); // Connect on clicking the button
        }
    }

    public void OnDiscoveredServer(DiscoveryResponse info)
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
