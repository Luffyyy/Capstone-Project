using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class MenuManager : MonoBehaviour
{
    public List<MenuBase> MenuPrefabs;
    public List<MenuBase> Menus = new();

    public Stack<MenuBase> MenuStack = new();

    public Stack<Dialog> DialogStack = new();
    
    public Transform SafeArea;

    public string StartMenu;

    public static MenuManager Instance { get; private set; }

    public bool IsActive;

    public bool AllowClosing = false;

    void Awake()
    {
        // Fixes 30 fps lock
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

        Instance = this;

        foreach (var menu in Menus)
        {
            menu.Hide();
        }

        if (!string.IsNullOrWhiteSpace(StartMenu))
        {
            OpenMenu(StartMenu);
        }

        if (IsActive)
        {
            Show();
        } else
        {
            Hide();
        }
    }

    public MenuBase AddMenu(string name)
    {
        var menu = Instantiate(MenuPrefabs.Find(menu => menu.gameObject.name == name), SafeArea);
        menu.Hide();
        menu.name = name;
        Menus.Add(menu.GetComponent<MenuBase>());
        return menu;
    }

    // Returns a menu stored in the menu manager
    public MenuBase GetMenu(string name)
    {
        return GetMenu<MenuBase>(name);
    }

    // Returns a menu stored in the menu manager and allows to quickly upcast it to some type you need
    public T GetMenu<T>(string name)
    {
        
        var menu = Menus.Find(menu => menu.name == name);
        if (menu is T tMenu)
        {
            return tMenu;
        } else
        {
            return default;
        }
    }


    public MenuBase OpenMenu(string name)
    {
        MenuBase menu = GetMenu(name);
        if (menu == null)
        {
            menu = AddMenu(name);
        }
        if (MenuStack.Count > 0)
        {
            MenuStack.Peek().Hide();
        }
        MenuStack.Push(menu);
        menu.Show();

        if (!IsActive)
        {
            Show();
        }
        return menu;
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed && !TouchScreenKeyboard.visible) // Avoid closing the menu if keyboard is up...
        {
            CloseCurrentMenu();
        }
    }

    public void CloseCurrentMenu()
    {
        if (DialogStack.Count > 0)
        {
            DialogStack.Peek().Hide();
            return;
        }
    
        var count = MenuStack.Count;
        if (count == 0 || (count <= 1 && !AllowClosing))
            return;

        MenuStack.Pop().Hide();
        if (count > 1)
        {
            MenuStack.Peek().Show();
        } else // Only possible if ALlowClosing = true
        {
            Hide();
        }
    }

    public void Show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        IsActive = true;

        if (HUDManager.Instance != null)
        {
            StartCoroutine(nameof(SetPlayerInputActiveNextFrame));
        }
    }

    public void Hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        IsActive = false;

        if (HUDManager.Instance != null)
        {
            StartCoroutine(nameof(SetPlayerInputActiveNextFrame));
        }
    }

    IEnumerator SetPlayerInputActiveNextFrame()
    {
        yield return null; // Wait for next frame
        GetComponent<PlayerInput>().enabled = IsActive;
        HUDManager.Instance.SetActive(!IsActive);
    }
}
