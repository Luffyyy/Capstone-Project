using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuBase : MonoBehaviour
{
    public bool IsActive = false;

    public virtual void Show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        IsActive = true;
    }

    public virtual void Hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        IsActive = false;
    }

    public void Back()
    {
        MenuManager.Instance.CloseCurrentMenu();
    }
}