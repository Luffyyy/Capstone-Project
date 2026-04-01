using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuBase : MonoBehaviour
{
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

    public void Back()
    {
        MenuManager.Instance.CloseCurrentMenu();
    }
}