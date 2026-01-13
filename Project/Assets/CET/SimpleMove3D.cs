using Mirror;
using UnityEngine;

public class SimpleMove3D : NetworkBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody rb;
    Vector3 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        input = new Vector3(h, 0f, v);
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        rb.linearVelocity = new Vector3(
            input.x * moveSpeed,
            rb.linearVelocity.y,
            input.z * moveSpeed
        );
    }
}
