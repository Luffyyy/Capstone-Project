using Mirror;

// Simple "entity" game object which are basically any object that you can interact with on the map
// If no port is defined set port as -1
public class Entity : NetworkBehaviour
{
    public int Port = -1;
}