using UnityEngine;

public class DualPasswordLock : BaseLock
{
    public BaseLock Lock1;
    public BaseLock Lock2;

    void Start()
    {
        Lock1.LockStateChanged.AddListener(locked => LockStateChanged.Invoke(IsLocked()));
        Lock2.LockStateChanged.AddListener(locked => LockStateChanged.Invoke(IsLocked()));
    }

    public override bool IsLocked()
    {
        return Lock1.IsLocked() || Lock2.IsLocked();
    }
}
