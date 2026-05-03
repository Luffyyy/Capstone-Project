using System.Collections;
using Mirror;
using UnityEngine;

public class Door : Activatable
{
    public Animator animator;
    
    public bool IsOpenTriggered = true;

    void Start()
    {
        SetIsOn(IsOn);
    }

    protected override void OnIsOnChanged(bool oldValue, bool newValue)
    {
        base.OnIsOnChanged(oldValue, newValue);
        if (!IsOpenTriggered) // This means the door is open the moment we turn on the object
        {
            animator.SetBool("isOpen", newValue);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOn && IsOpenTriggered) {
            if (other.CompareTag("Player"))
            {   
                animator.SetBool("isOpen", true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && IsOpenTriggered)
        {
            animator.SetBool("isOpen", false);
        }
    }
}
