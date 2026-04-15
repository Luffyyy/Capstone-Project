using System.Collections;
using Mirror;
using UnityEngine;

public class OpenDoors : Activatable
{
    public Animator animator;
    void Start()
    {
        SetEmission(IsOn);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsOn){
            if (other.CompareTag("Player"))
            {   
                animator.SetBool("isOpen", true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isOpen", false);
        }
    }
}
