using UnityEngine;

public class DualPasswordLock : BaseLock
{
    public BaseLock Lock1;
    public BaseLock Lock2;

    void Start()
    {
        Lock1.LockStateChanged.AddListener(locked => LockStateChanged.Invoke(IsUnlocked()));
        Lock2.LockStateChanged.AddListener(locked => LockStateChanged.Invoke(IsUnlocked()));
    }

    public override bool IsUnlocked()
    {
        return Lock1.IsUnlocked() && Lock2.IsUnlocked();
    }
}
