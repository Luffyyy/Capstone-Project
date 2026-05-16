using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public Animator animator;

    public Transform DialogWindow;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTitle(string title)
    {
        Title.text = title;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        animator.SetBool("IsShowing", true);
        MenuManager.Instance.DialogStack.Push(this);
    }

    public void Hide()
    {
        MenuManager.Instance.DialogStack.Pop();
        animator.SetBool("IsShowing", false);
    }

    public void FinishHiding()
    {
        gameObject.SetActive(false);
    }
}
