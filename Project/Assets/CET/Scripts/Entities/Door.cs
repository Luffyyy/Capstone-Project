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

    public override void SetIsOn(bool value)
    {
        base.SetIsOn(value);
        if (!IsOpenTriggered) // This means the door is open the moment we turn on the object
        {
            animator.SetBool("isOpen", value);
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
