using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using NUnit.Framework;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    private Vector2 move;
    public Interactable CurrentInteractable;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || MenuManager.Instance.IsActive) return;
        move = context.ReadValue<Vector2>();
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !context.performed || MenuManager.Instance.IsActive) return;

        if (CurrentInteractable is Interactable i)
        {
            //TODO: send requet to server to see whether we are allowed to interact. 
            // For example what if someone else is interacting with the terminal?
            i.Interact();
            // CmdInteract(t.gameObject);
        }
    }

    [Command]
    void CmdInteract(GameObject target)
    {
        if (target.TryGetComponent<Interactable>(out var i))
        {
            i.Interact();
        }
    }   

    void Update()
    {
        if (isLocalPlayer)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new(move.x,0f,move.y);
        transform.Translate(speed * Time.deltaTime * movement, Space.World);
        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 15f);
        }
    }
}
