using System.Collections;
using Mirror;
using NUnit.Framework;
using UnityEngine;

public class Door : Activatable
{
    public Animator animator;
    public bool IsLastDoor = false;
    public bool IsOpenTriggered = true;

    protected override void Awake()
    {
        base.Awake();
        SetIsOn(IsOn);
    }

    protected override void OnIsOnChanged(bool oldValue, bool newValue)
    {
        base.OnIsOnChanged(oldValue, newValue);
        Debug.Log($"Door hook {oldValue}->{newValue}");
        if (!IsOpenTriggered) // This means the door is open the moment we turn on the object
        {
            Debug.Log("Setting animator isOpen = " + newValue);
            animator.SetBool("isOpen", newValue);
        }
        if (IsLastDoor && newValue)
        {
            ServerHUD.Instance.PlayFinishedLevelHud();
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
