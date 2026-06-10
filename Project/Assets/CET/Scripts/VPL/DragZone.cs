using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragZone : MonoBehaviour, IDropHandler
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        animator.SetBool("IsShowing", true);
    }

    public void Hide()
    {
        animator.SetBool("IsShowing", false);
    }

    public abstract void OnDrop(PointerEventData eventData);
}
