using UnityEngine;

public class PasswordLock : BaseLock
{
    public string Password;

    private bool isLocked = false;

    public void EnterPassword(string password)
    {
        isLocked = Password != password;

        LockStateChanged.Invoke(isLocked);
    }
    public override bool IsLocked()
    {
        return isLocked;
    }
}
