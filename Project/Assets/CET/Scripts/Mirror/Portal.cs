using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : NetworkBehaviour
{
    [Scene, Tooltip("Which scene to send player from here")]
    public string destinationScene;
    public Dialog Dialog;
    public bool endOfGame = false;
    public static Portal Instance;

    int numPlayers;
    void  Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        // ignore CharacterController colliders
        if (!isServer || !other.CompareTag("Player") || other is not CapsuleCollider) return;

        if (++numPlayers == 2)
        {
            if (endOfGame)
            {
                Debug.Log("End of game reached, showing dialog");
                MenuManager.Instance.ShowDialog("LastDialog");
            } else
            {
                NewNetworkManager.singleton.ChangeLevel(destinationScene);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isServer || !other.CompareTag("Player")) return;

        numPlayers--;
    }
    public void LastDialog()
    {
        if (NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                NetworkManager.singleton.StopHost();
            }
            {
                NetworkManager.singleton.StopServer();
            }
        } else
        {
            NetworkManager.singleton.StopClient();
        }
    }
}