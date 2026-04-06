using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ServerHUD : HUDBase
{
    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuManager.Instance.OpenMenu("PauseMenu");
        }
    }
}