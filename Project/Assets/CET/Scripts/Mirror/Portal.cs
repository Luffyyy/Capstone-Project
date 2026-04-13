using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : NetworkBehaviour
{
    [Scene, Tooltip("Which scene to send player from here")]
    public string destinationScene;

    int numPlayers;

    void OnTriggerEnter(Collider other)
    {
        // ignore CharacterController colliders
        if (!isServer || !other.CompareTag("Player") || other is not CapsuleCollider) return;

        if (++numPlayers == 2)
        {
            NewNetworkManager.singleton.ChangeLevel(destinationScene);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isServer || !other.CompareTag("Player")) return;

        numPlayers--;
    }
}