using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class MenuManager : MonoBehaviour
{
    public List<MenuBase> MenuPrefabs;
    public List<MenuBase> Menus = new();

    public Stack<MenuBase> MenuStack = new();
    
    public Transform SafeArea;

    public string StartMenu;

    public static MenuManager Instance { get; private set; }

    public bool IsActive;

    public bool AllowClosing = false;

    void Awake()
    {
        Instance = this;
        if (!string.IsNullOrWhiteSpace(StartMenu))
        {
            OpenMenu(StartMenu);
        }
    }

    public void OpenMenu(string name)
    {
        MenuBase menu = Menus.Find(menu => menu.name == name);
        if (menu == null)
        {
            menu = Instantiate(MenuPrefabs.Find(menu => menu.gameObject.name == name), SafeArea);
            menu.name = name;
            Menus.Add(menu.GetComponent<MenuBase>());
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
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CloseCurrentMenu();
        }
    }

    public void CloseCurrentMenu()
    {
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
            HUDManager.Instance.SetActive(false);
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
            HUDManager.Instance.SetActive(true);
        }
    }
}
