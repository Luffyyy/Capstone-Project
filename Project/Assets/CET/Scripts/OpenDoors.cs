using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            Debug.Log("TRIGGER WORKED with: " + other.name);
            animator.SetBool("isOpen", true);
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
