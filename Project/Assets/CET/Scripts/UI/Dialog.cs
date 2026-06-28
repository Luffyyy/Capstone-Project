using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public Animator animator;

    public Transform DialogWindow;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        MenuManager.Instance.AddDialog(this);
    }

    public void SetTitle(string title)
    {
        Title.text = title;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        animator.SetBool("IsShowing", true);
        MenuManager.Instance.PushDialog(this);
    }

    public virtual void Hide()
    {
        MenuManager.Instance.PopDialog();
        animator.SetBool("IsShowing", false);
    }

    public void FinishHiding()
    {
        gameObject.SetActive(false);
    }
}
