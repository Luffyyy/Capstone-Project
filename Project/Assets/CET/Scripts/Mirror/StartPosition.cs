using Mirror;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public int PlayerIndex;
    public void Awake()
    {
        RegisterStartPosition();
    }

    void RegisterStartPosition()
    {
        NetworkManager.RegisterStartPosition(transform);
    }
}
