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

        NewNetworkManager.OnChangeLevel.AddListener(level => SetCurrentHUD());
        OpenHUD("GlobalHUD");
    }

    public void SetCurrentHUD()
    {
        if (GameState.Instance.InLobby())
        {
            CloseHUD("PlayerHUD");
            CloseHUD("ServerHUD");
            return;
        }
        // Either host mode or server itself
        if (!GameState.Instance.IsDedicatedServer || GameState.Instance.isServerOnly)
        {
            OpenHUD("ServerHUD");
        } else
        {
            CloseHUD("ServerHUD");
        }
        if (!GameState.Instance.isServerOnly)
        {
            OpenHUD("PlayerHUD");
            //Show the background only on clients. TODO: maybe have it optional?
            GetComponent<Image>().enabled = GameState.Instance.IsDedicatedServer && NetworkClient.active;
        } else
        {
            CloseHUD("PlayerHUD");
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
        hud.Show();
    }

    public void CloseHUD(string name)
    {
        HUDBase hud = HUDs.Find(hud => hud.name == name);
        if (hud != null)
        {
            HUDs.Remove(hud);
            Destroy(hud.gameObject);
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
        } else if (ServerHUD.Instance != null)
        {
            ServerHUD.Instance.GetComponent<PlayerInput>().enabled = IsActive;
        }

        foreach (var hud in HUDs)
        {
            if (IsActive)
            {
                hud.Show();
            } else
            {
                hud.Hide();
            }
            // hud.gameObject.SetActive(IsActive);
        }
    }
}