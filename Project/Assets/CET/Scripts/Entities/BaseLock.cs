using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLock : NetworkBehaviour
{
    public UnityEvent<bool> LockStateChanged;
    public abstract bool IsLocked();
}
