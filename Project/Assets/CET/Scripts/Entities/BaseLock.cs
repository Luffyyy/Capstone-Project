using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLock : NetworkBehaviour
{
    public UnityEvent<bool> LockStateChanged;

    [SyncVar(hook=nameof(OnIsUnlockedChanged))]
    protected bool isUnlocked = false;

    public void OnIsUnlockedChanged(bool oldVal, bool newVal)
    {
        LockStateChanged.Invoke(newVal);
    }

    public virtual bool IsUnlocked()
    {
        return isUnlocked;
    }
}
