using UnityEngine;
using Mirror;

public class NewNetworkManager : NetworkManager
{
    LevelManager levelManager;
    private Vector3 player1pos;
    private Vector3 player2pos;
    public override void OnServerSceneChanged(string sceneName)
    {
        levelManager = FindObjectOfType<LevelManager>();

        if (levelManager == null )
        {
            Debug.LogError("Spawn points not found!");
            return;
        }
        var p1 = levelManager.GetPlayer1Spawn();
        var p2 = levelManager.GetPlayer2Spawn();

        if (p1 == null || p2 == null)
        {
            Debug.LogError("Spawn points not found!");
            return;
        }

        player1pos = p1.position;
        player2pos = p2.position;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Vector3 spawnPos;
        GameObject player;
        if (numPlayers == 0)
        {
            spawnPos = player1pos;
        }
        else
        {
            spawnPos = player2pos;
        }

        player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}