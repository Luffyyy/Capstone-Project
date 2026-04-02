using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class HUDManager : MonoBehaviour
{
    public List<HUDBase> HUDPrefabs;
    public List<HUDBase> HUDs = new();

    public Transform SafeArea;

    public static HUDManager Instance { get; private set; }

    public bool IsActive = true;

    void Start()
    {
        Instance = this;
    }

    public void SetCurrentHUD(bool IsDedicatedServer, bool isClient)
    {
        // Set background on/off depending on whether we are on a dedicated server screen
        if (IsDedicatedServer)
        {
            if (isClient)
            {
                OpenHUD("PlayerHUD");
                GetComponent<Image>().enabled = true; //Show the background only on clients. TODO: maybe have it optional?
            } else
            {
                OpenHUD("ServerHUD");
            }
        } else
        {
            OpenHUD("PlayerHUD");
        }
    }

    public void OpenHUD(string name)
    {
        HUDBase hud = HUDs.Find(hud => hud.name == name);

        if (hud == null)
        {
            hud = Instantiate(HUDPrefabs.Find(menu => menu.gameObject.name == name), SafeArea);
            hud.name = name;
            HUDs.Add(hud.GetComponent<HUDBase>());
        }
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuManager.Instance.OpenMenu("PauseMenu");
        }
    }


    public void SetActive(bool active)
    {
        IsActive = active;
        GetComponent<PlayerInput>().enabled = active;
    }
}