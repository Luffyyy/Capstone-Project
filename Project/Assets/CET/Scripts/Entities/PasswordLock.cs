using Mirror;
using UnityEngine;

public class PasswordLock : BaseLock
{
    public string Password;

    public void EnterPassword(string password)
    {
        isUnlocked = Password == password;

        LockStateChanged.Invoke(isUnlocked);
    }
}
