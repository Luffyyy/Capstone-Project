using UnityEngine;
using Mirror;
using Mirror.Discovery;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;
/**
The default network manager does not support additive scenes out of the box
So to solve this we need to understand how the levels in CET will be loaded
Generally the idea is to load a level as we need it. Possibly unload the ones we don't need?
Depending on how fast it is, perhaps load 2 levels at once so the players can progress through levels without much hiccups.

Another idea that is floating in my head, in games like portal there are chapters and sub-levels in there like chamber 01, chamber 02
So we load all levels in a chapter (or when needed one-by-one) and unload when loading a new chapter, but this really depends how many levels we'll have...
*/
public class NewNetworkManager : NetworkManager
{
    // Sent by server before despawning players/unloading scenes so clients can hide the transition.
    public struct BeginLevelTransitionMessage : NetworkMessage { }
    public static new NewNetworkManager singleton => (NewNetworkManager)NetworkManager.singleton;

    [Tooltip("A scene that holds things such as player HUD, menus, etc")]
    public string GameScene;

    [Tooltip("The currently loaded level")]
    public string CurrentLevel { get; private set; }

    [Tooltip("Reference to FadeInOut script on child FadeCanvas")]
    public FadeInOut fadeInOut;

    [Tooltip("How long the server waits after notifying clients to start fading before despawning players.")]
    [Min(0f)]
    public float clientTransitionLeadTime = 0.15f;

    public List<SceneCamera> SceneCameras;

    public static UnityEvent<string> OnChangeLevel = new();

    public int GetPlayerIndex()
    {
        if (numPlayers > 0)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn?.identity != null)
                {
                    var index = conn.identity.GetComponent<Player>().PlayerIndex;
                    return (index+1)%2;
                }
            }
        }

        return Random.Range(0, 2);
    }

    public SceneCamera GetSceneCamera(string scene)
    {
        foreach (SceneCamera cam in SceneCameras)
        {
            if (cam.gameObject.scene.path == scene) {
                return cam;
            }
        }
        return null;
    }

    public static Transform GetStartPosition(int playerIndex, string scene=null)
    {
        foreach (var pos in startPositions)
        {
            var sp = pos.GetComponent<StartPosition>();
            if (sp != null && sp.PlayerIndex == playerIndex && (scene == null || sp.gameObject.scene.path == scene))
            {
                return pos;
            }
        }
        return null;
    }

    public GameObject SpawnPlayer(NetworkConnectionToClient conn, PlayerSaveData oldPlayer=null)
    {
        // We have Network Start Positions in first additive scene...pick one

        // Instantiate player as child of start position - this will place it in the additive scene
        // This also lets player object "inherit" pos and rot from start position transform
        var index = oldPlayer != null ? oldPlayer.PlayerIndex : GetPlayerIndex();
        var startTransform = GetStartPosition(index);
        GameObject player = Instantiate(playerPrefab, startTransform.position, startTransform.rotation, new InstantiateParameters()
        {
            scene = startTransform.gameObject.scene
        });
        // now set parent null to get it out from under the Start Position object
        // player.transform.SetParent(null, true);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

        var playerComp = player.GetComponent<Player>();
        playerComp.PlayerIndex = index;

        playerComp.SetColorIndex();
        playerComp.SetEmotionIndex(oldPlayer != null ? oldPlayer.EmotionIndex : (numPlayers + Random.Range(0, 10)) % 10);
        playerComp.Username = oldPlayer != null ? oldPlayer.Username : "p"+(index+1);
        if (GameState.Instance.InLobby())
        {
            LobbyManager.Instance.ReadyStates[index] = ReadyState.Unready;
        }

        return player;
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<BeginLevelTransitionMessage>();
        base.OnStopClient();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<BeginLevelTransitionMessage>(OnBeginLevelTransition, false);
    }

    private void OnBeginLevelTransition(BeginLevelTransitionMessage _)
    {
        // Host already performs transition visuals in the server path.
        if (mode == NetworkManagerMode.Host || isInTransition)
            return;

        StartCoroutine(fadeInOut.FadeIn());
    }

    // Triggered when a player disconnects from the server
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (GameState.Instance.InLobby())
        {
            LobbyManager.Instance.ReadyStates[conn.identity.GetComponent<Player>().PlayerIndex] = ReadyState.Offline;
        }
        base.OnServerDisconnect(conn);
    }

    #region Scene Management
    // Additive levels code (Taken from examples)

    // This is set true after server loads all subscene instances
    private bool subscenesLoaded;

    [Scene, Tooltip("Add additive scenes here.\nFirst entry will be players' start scene")]
    public string[] additiveScenes;

    // This is managed in LoadAdditive, UnloadAdditive, and checked in OnClientSceneChanged
    bool isInTransition;

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName)
    {
        // This fires after server fully changes scenes, e.g. offline to online
        // If server has just loaded the Container (online) scene, load the subscenes on server
        if (sceneName == onlineScene)
        {
            ChangeLevel(additiveScenes[0], true);
        }
    }

    public bool ChangeLevel(string sceneName, bool initial=false)
    {
        if (CurrentLevel == sceneName) return false;

        if (VPLMenu.Instance != null)
        {
            VPLMenu.Instance.DestroyZones();
        }

        StartCoroutine(AsyncChangeLevel(sceneName, initial));
        return true;
    }

    public IEnumerator AsyncChangeLevel(string sceneName, bool initial=false)
    {
        subscenesLoaded = false; // Wait before spawning new players

        var oldLevel = CurrentLevel;
        CurrentLevel = sceneName;

        // Ask clients to fade in before we despawn players and unload scenes.
        if (!initial)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                // Do not send to host-local connection.
                if (conn is LocalConnectionToClient)
                    continue;

                conn.Send(new BeginLevelTransitionMessage());
            }
            if (clientTransitionLeadTime > 0f)
                yield return new WaitForSeconds(clientTransitionLeadTime);
            else
                yield return null;
        }

        // Hide the mess from players
        yield return fadeInOut.FadeIn(initial);

        // Remove players after fader has completed
        Dictionary<int, PlayerSaveData> saveData = new();
        if (!initial)
        {
            foreach (var pair in NetworkServer.connections)
            {
                if (pair.Value.identity is NetworkIdentity identity)
                {
                    saveData.Add(pair.Key, identity.GetComponent<Player>().GetData());
                }
                NetworkServer.RemovePlayerForConnection(pair.Value, RemovePlayerOptions.Unspawn);
            }
        }

        // Remove old start positions so players spawn on new level
        startPositions.Clear();
        SceneCameras.Clear();

        // Unload old level
        if (!string.IsNullOrEmpty(oldLevel))
        {
            yield return SceneManager.UnloadSceneAsync(oldLevel);
            yield return Resources.UnloadUnusedAssets();
            // Tell clients to unload old level
            NetworkServer.SendToAll(new SceneMessage { sceneName = oldLevel, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });
        }

        // Load new level
        yield return SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);

        // Server mode view
        if (mode != NetworkManagerMode.Host)
        {
            var cam = GetSceneCamera(CurrentLevel);
            if (cam != null)
            {
                GameObject.Find("MainCamera").transform.SetPositionAndRotation(cam.transform.position, cam.transform.rotation);
            }
        }

        subscenesLoaded = true;

        if (!initial)
        {
            // Tell clients to load the new level
            NetworkServer.SendToAll(new SceneMessage { sceneName = CurrentLevel, sceneOperation = SceneOperation.LoadAdditive, customHandling = true });

            yield return new WaitForEndOfFrame();

            // Handle player objects
            foreach (var pair in NetworkServer.connections)
            {
                var player = SpawnPlayer(pair.Value, saveData[pair.Key]);
                NetworkServer.AddPlayerForConnection(pair.Value, player);
            }

        } // else handled in OnServerReady

        // All ready
        yield return fadeInOut.FadeOut();
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        if (newSceneName == offlineScene)
        {
            StartCoroutine(fadeInOut.FadeIn(true));
        }
    }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="sceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string sceneName, SceneOperation sceneOperation, bool customHandling)
    {
        //Debug.Log($"{System.DateTime.Now:HH:mm:ss:fff} OnClientChangeScene {sceneName} {sceneOperation}");
        if (sceneName == offlineScene || sceneName == onlineScene)
        {
            StartCoroutine(fadeInOut.FadeIn(true));
        }

        if (sceneOperation == SceneOperation.UnloadAdditive)
            StartCoroutine(UnloadAdditive(sceneName));

        if (sceneOperation == SceneOperation.LoadAdditive)
            StartCoroutine(LoadAdditive(sceneName));
    }


    IEnumerator LoadAdditive(string sceneName)
    {
        // Fixes 30 fps lock
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

        isInTransition = true;

        // This will return immediately if already faded in
        // e.g. by UnloadAdditive or by default startup state
        yield return fadeInOut.FadeIn();

        // host client is on server...don't load the additive scene again
        if (mode == NetworkManagerMode.ClientOnly)
        {
            CurrentLevel = sceneName;
            // Start loading the additive subscene
            loadingSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
                yield return null;
        }

        // Reset these to false when ready to proceed
        NetworkClient.isLoadingScene = false;
        isInTransition = false;

        OnClientSceneChanged();

        OnChangeLevel.Invoke(sceneName);

        // Reveal the new scene content.
        yield return fadeInOut.FadeOut();
    }

    IEnumerator UnloadAdditive(string sceneName)
    {
        isInTransition = true;

        // This will return immediately if already faded in
        // e.g. by LoadAdditive above or by default startup state.
        yield return fadeInOut.FadeIn();

        // host client is on server...don't unload the additive scene here.
        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        // Reset these to false when ready to proceed
        NetworkClient.isLoadingScene = false;
        isInTransition = false;

        OnClientSceneChanged();
    }
    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        // This fires from a Ready message client sends to server after loading the online scene
        base.OnServerReady(conn);

        if (conn.identity == null)
            StartCoroutine(AddPlayerDelayed(conn));
    }

    // This delay is mostly for the host player that loads too fast for the
    // server to have subscenes async loaded from OnServerSceneChanged ahead of it.
    IEnumerator AddPlayerDelayed(NetworkConnectionToClient conn)
    {
        // Wait for server to async load all subscenes for game instances
        while (!subscenesLoaded)
            yield return null;

        // Send Scene msg to client telling it to load the current level
        conn.Send(new SceneMessage { sceneName = CurrentLevel, sceneOperation = SceneOperation.LoadAdditive, customHandling = true });
        var player = SpawnPlayer(conn);
        // Wait for end of frame before adding the player to ensure Scene Message goes first
        yield return new WaitForEndOfFrame();
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    #endregion
}