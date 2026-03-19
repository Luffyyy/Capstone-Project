using UnityEngine;

public class BasicMoves : MonoBehaviour
{
    public float speed = 5f;
    void Start()
    {

    }
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        if(moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 15f);
        }
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}