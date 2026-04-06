using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using NUnit.Framework;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    public float speed;
    private Vector2 move;
    public Interactable CurrentInteractable;
    Rigidbody rb;

    public bool IsInputEnabled = true;

    public void SetInputEnabled(bool enabled)
    {
        IsInputEnabled = enabled;
        GetComponent<PlayerInput>().enabled = enabled;
        move = Vector2.zero; // Stop player
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || MenuManager.Instance.IsActive) return;
        move = context.ReadValue<Vector2>();
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            var op = GetComponent<PlayerInput>().currentActionMap.FindAction("OpenPauseMenu");
            op.performed += HUDManager.Instance.OpenPauseMenu;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !context.performed || MenuManager.Instance.IsActive || !IsInputEnabled) return;

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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isLocalPlayer && IsInputEnabled)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new(move.x,0f,move.y);
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        //transform.Translate(speed * Time.deltaTime * movement, Space.World);
        if(movement != Vector3.zero)
        {  
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 15f);
        }
    }
}
