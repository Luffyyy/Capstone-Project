using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public void SetTitle(string title)
    {
        Title.text = title;
    }

    public void Show()
    {
        MenuManager.Instance.DialogStack.Push(this);
        gameObject.SetActive(true);       
    }

    public void Hide()
    {
        MenuManager.Instance.DialogStack.Pop();
        gameObject.SetActive(false);
    }
}
