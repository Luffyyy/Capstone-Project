using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;
    public Transform GetPlayer1Spawn()
    {
        return player1Spawn;
    }

    public Transform GetPlayer2Spawn()
    {
        return player2Spawn;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void enableDoor(GameObject door)
    {
        
    }
}
