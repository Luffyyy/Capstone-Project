using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    [SyncVar]
    public bool IsDedicatedServer;

    public static GameState Instance;

    [SyncVar]
    public bool[] Collected = new bool[4];

    void Awake()
    {
        Instance = this;
    }

    public void Collect(CollectableType type)
    {
        int i = (int)type;
        Collected[i] = true;
        Collected = (bool[])Collected.Clone(); // A small hack to force sync the array
        ControlJournal.Instance.Collect(type);

        ClientCollect(i);
    }

    [ClientRpc]
    public void ClientCollect(int type)
    {
        ControlJournal.Instance.Collect((CollectableType)type);
    }

    public override void OnStartClient()
    {
        Instance = this;
        HUDManager.Instance.SetCurrentHUD();

        VolumeController.Instance.SetDefaults();
        if (IsDedicatedServer)
        {
            // On phones we will only play UI sounds
            VolumeController.Instance.SetVolume("Music", 0);
            VolumeController.Instance.SetVolume("SFX", 0);

            // Disable rendering since we aren't supposed to see anything
            GameObject.Find("MainCamera").GetComponent<Camera>().cullingMask = 0;
        }

        for (int i=0; i<Collected.Length; i++)
        {
            if (Collected[i])
            {
                ControlJournal.Instance.Collect((CollectableType)i);
            }
        }
    }

    public override void OnStartServer()
    {
        IsDedicatedServer = !isClient;
        HUDManager.Instance.SetCurrentHUD();
        VolumeController.Instance.SetDefaults();
    }

    public bool InLobby()
    {
        var currLevel = NewNetworkManager.singleton.CurrentLevel ?? "";
        return currLevel.EndsWith("LobbyScene.unity");
    }
}