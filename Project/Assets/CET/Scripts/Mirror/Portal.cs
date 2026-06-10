using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : NetworkBehaviour
{
    [Scene, Tooltip("Which scene to send player from here")]
    public string destinationScene;
    public static Portal Instance;

    public Transform TeleportPoint;

    [SyncVar(hook=nameof(OnNumPlayersUpdated))]
    int numPlayers;
    void  Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || other is not CapsuleCollider) return;

        if (isClient) {
            other.GetComponent<PlayerController>().SetInputEnabled(false);
            if (TeleportPoint != null)
            {
                other.GetComponent<CharacterController>().enabled = false;
                other.transform.position = TeleportPoint.position;
            }
        }

        if (!isServer) {
            return;
        }

        ServerHUD.Instance.levelFinished.UpdateEscapedPlayers(++numPlayers);

        if (numPlayers == 2)
        {
            ServerHUD.Instance.levelFinished.Hide();
            NewNetworkManager.singleton.ChangeLevel(destinationScene);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isServer || !other.CompareTag("Player")) return;

        ServerHUD.Instance.levelFinished.UpdateEscapedPlayers(--numPlayers);
    }

    public void OnNumPlayersUpdated(int oldVal, int newVal)
    {
        ServerHUD.Instance.levelFinished.UpdateEscapedPlayers(newVal);
    }
}