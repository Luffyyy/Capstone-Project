using UnityEngine;

public class DualPasswordLock : BaseLock
{
    public BaseLock Lock1;
    public BaseLock Lock2;

    void Start()
    {
        Lock1.LockStateChanged.AddListener(unlocked => LocksChanged());
        Lock2.LockStateChanged.AddListener(unlocked => LocksChanged());
    }

    private void LocksChanged()
    {
        isUnlocked = Lock1.IsUnlocked() && Lock2.IsUnlocked();
        LockStateChanged.Invoke(isUnlocked);
    }
}
