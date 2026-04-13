
using Mirror;
using UnityEngine;
public class CameraTransition : MonoBehaviour
{
    public float Speed;
    public Transform target;

    public void MoveTo(SceneCamera cam)
    {
        target = cam.transform;

        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - 3, pos.z);
    }

    public void Update()
    {
        if (target != null)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, Speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Speed * Time.deltaTime);
        }
    }
}