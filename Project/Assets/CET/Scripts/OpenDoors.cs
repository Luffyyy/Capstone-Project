using UnityEngine;

public class OpenDoors : Activatable
{
    public Animator animator;
    void Start()
    {
        Type = "Door";
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(IsOn){
            if (other.CompareTag("Player"))
            {   
                animator.SetBool("isOpen", true);
            }
        }//ג
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isOpen", false);
        }
    }
}
