using UnityEngine;

public class Dialog : MonoBehaviour
{
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
