using System.Collections;
using System.Collections.Generic;
using Mirror;
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
        SetCurrentHUD();
    }

    public void SetCurrentHUD()
    {
        // Set background on/off depending on whether we are on a dedicated server screen
        if (GameState.Instance.IsDedicatedServer)
        {
            if (NetworkClient.active)
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

        StartCoroutine(nameof(HandleSetActiveNextFrame));
    }

    // This is done in the next frame because unity's input system does not like to be disabled in the middle of a keypress
    // Why is it like this? Genuine question
    IEnumerator HandleSetActiveNextFrame() {
        yield return null; // Wait for next frame
        var localPlayer = NetworkClient.localPlayer;
        if (localPlayer != null)
        {
            localPlayer.GetComponent<PlayerController>().SetInputEnabled(IsActive);
        }

        foreach (var hud in HUDs)
        {
            hud.gameObject.SetActive(IsActive);
        }
    }
}