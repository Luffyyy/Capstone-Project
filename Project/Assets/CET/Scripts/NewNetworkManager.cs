using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class NewNetworkManager : NetworkManager
{
    LevelManager levelManager;
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

        RegisterStartPosition(p1);
        RegisterStartPosition(p2);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;

        player = Instantiate(playerPrefab, startPositions[numPlayers].position, startPositions[numPlayers].rotation);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

        player.GetComponent<PlayerCustomizer>().SetColorIndex((numPlayers + Random.Range(0, 10)) % 10);
        player.GetComponent<PlayerCustomizer>().SetEmotionIndex((numPlayers + Random.Range(0, 10)) % 10);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}