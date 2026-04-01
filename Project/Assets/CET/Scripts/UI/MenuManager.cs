using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuManager : MonoBehaviour
{
    public List<MenuBase> MenuPrefabs;
    public List<MenuBase> Menus = new();

    public Stack<MenuBase> MenuStack = new();
    
    public Transform SafeArea;

    public string StartMenu;

    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        if (StartMenu != null)
        {
            OpenMenu(StartMenu);
        }
    }

    public void OpenMenu(string name)
    {
        print(Menus);
        MenuBase menu = Menus.Find(menu => menu.name == name);
        if (menu == null)
        {
            menu = Instantiate(MenuPrefabs.Find(menu => menu.gameObject.name == name), SafeArea);
            Menus.Add(menu.GetComponent<MenuBase>());
        }
        if (MenuStack.Count > 0)
        {
            MenuStack.Peek().Hide();
        }
        MenuStack.Push(menu);
        menu.Show();
    }

    public void CloseCurrentMenu()
    {
        if (MenuStack.Count <= 1)
            return;

        MenuStack.Pop().Hide();
        MenuStack.Peek().Show();
    }

    public void Show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
    }

    public void Hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
    }
}
