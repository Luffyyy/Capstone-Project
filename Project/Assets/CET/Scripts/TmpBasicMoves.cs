using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class TmpBasicMoves : NetworkBehaviour
{
    public float speed;
    private Vector2 move;
    public ShowUiOnTerminal currentInteractable;
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        move = context.ReadValue<Vector2>();
    
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact pressed11");
        if (!isLocalPlayer) return;
        Debug.Log("Interact pressed12");
        if (!context.performed) return;

        Debug.Log("Interact pressed1");

        if (currentInteractable is ShowUiOnTerminal t)
        {
            Debug.Log("Interact pressed0");
            //TODO: send requet to server to see whether we are allowed to interact. 
            // For example what if someone else is interacting with the terminal?
            t.Interact();
            // CmdInteract(t.gameObject);
        }
    }
    [Command]
    void CmdInteract(GameObject target)
    {
        Debug.Log("Interact pressed2");
        var interactable = target.GetComponent<ShowUiOnTerminal>();
        
        if (interactable != null)
        {
            Debug.Log("Interact pressed3");
            interactable.Interact();
        }
    }   
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
            {
                movePlayer();
            }
    }
    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x,0f,move.y);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 15f);
        }
    }
}
