
using Mirror;
using UnityEngine;
public class CameraTransition : MonoBehaviour
{
    public float Speed;
    public Transform target;

    public void Update()
    {
        if (target != null)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, Speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Speed * Time.deltaTime);
        }
    }
}