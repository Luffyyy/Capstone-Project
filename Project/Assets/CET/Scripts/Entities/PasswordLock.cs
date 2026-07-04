using Mirror;
using UnityEngine;

public class PasswordLock : BaseLock
{
    public string Password;

    public void EnterPassword(string password)
    {
        bool wasUnlocked = isUnlocked;
        isUnlocked = Password == password;

        if (wasUnlocked != isUnlocked)
        {
            LockStateChanged.Invoke(isUnlocked);
        }
    }
}
