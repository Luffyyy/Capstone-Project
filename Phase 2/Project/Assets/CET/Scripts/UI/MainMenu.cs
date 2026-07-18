using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    readonly Dictionary<long, DiscoveryResponse> discoveredServers = new();

    private NewNetworkDiscovery networkDiscovery;

    public GameObject serverList;

    public Dialog serversDialog;
    public Dialog hostGameDialog;
    public Dialog ErrorDialog;
    public TextMeshProUGUI Error;

    public TMP_InputField ServerName;
    public Toggle ServerOnly;

    public GameObject serverButton;

    [HideInInspector] public string ServerNameStr;

    public static MainMenu Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        networkDiscovery = NetworkManager.singleton.GetComponent<NewNetworkDiscovery>();
        if (networkDiscovery != null)
        {
            networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
        }
        
        InvokeRepeating(nameof(ClearFoundServers), 0, 3);
    }

    // Clears discovered servers to ensure we don't show servers that aren't active anymore
    public void ClearFoundServers()
    {
        foreach (var pair in discoveredServers.ToList())
        {
            if (DateTime.Now - pair.Value.CreationDate > TimeSpan.FromSeconds(10)) // Delete if no updates for 10 seconds
            {
                discoveredServers.Remove(pair.Key);
            }
        }
        UpdateServerList();
        
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

        try
        {
            if (ServerOnly.isOn)
            {
                NetworkManager.singleton.StartServer();
            } else
            {
                NetworkManager.singleton.StartHost();
            }
            networkDiscovery.ServerName = ServerNameStr;
            networkDiscovery.AdvertiseServer();

            gameObject.SetActive(false);   
        }
        catch (Exception e)
        {
            Error.text = "Something went wrong while trying to host the game:\n" + e.Message;
            ErrorDialog.Show();
        }
    }

    // Starts looking for games and renders it in a list
    public void LookForGames()
    {
        serversDialog.Show();
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();

        UpdateServerList();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        MenuManager.Instance.OpenMenu("SettingsMenu");
    }

    public void Connect(DiscoveryResponse info)
    {
        networkDiscovery.StopDiscovery();
        try
        {
            NetworkManager.singleton.StartClient(info.uri);
            
        }
        catch (Exception e)
        {
            Error.text = "Something went wrong while trying to connect to the server:\n" + e.Message;
            ErrorDialog.Show();
            throw;
        }
        
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
            if (pair.Value.NumPlayers == 2) continue;

            var btn = Instantiate(serverButton, serverList.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().SetText( $"{pair.Value.Name} ({pair.Value.NumPlayers} Players)");
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
}
