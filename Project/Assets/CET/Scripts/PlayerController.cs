using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    public float speed;
    private Vector2 move;
    private Vector2 smoothedMove;

    private float side;
    private float smoothedSide;

    [SyncVar]
    public Interactable CurrentInteractable;

    public bool IsInputEnabled = true;

    private CharacterController charControl;
    private Rigidbody rb;
    private Animator animator;
    public bool CanMove = true;

    public GameObject InteractionText;
    public GameObject InteractionTextPrefab;

    public void SetInputEnabled(bool enabled)
    {
        IsInputEnabled = enabled;
        GetComponent<PlayerInput>().enabled = enabled;
        move = Vector2.zero; // Stop player
        side = 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || MenuManager.Instance.IsActive) return;
        move = context.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            side = Mathf.Sign(Vector3.Cross(transform.forward, new Vector3(move.x, 0, move.y)).y);
        } else
        {
            side = 0;
        }
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            var op = GetComponent<PlayerInput>().currentActionMap.FindAction("OpenPauseMenu");
            op.performed += HUDManager.Instance.OpenPauseMenu;
        } else
        {
            charControl.enabled = false;
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
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        InteractionText = Instantiate(InteractionTextPrefab, GameObject.Find("Canvas").transform);
        InteractionText.transform.SetSiblingIndex(0);

        // Reset movement just in case
        animator.SetFloat("Speed", 0);
        animator.SetFloat("Side", 0);

    }
    void Update()
    {
        if(!CanMove) return;
        if (isLocalPlayer)
        {
            smoothedMove = Vector2.Lerp(smoothedMove, move * speed, Time.deltaTime * 5);
            smoothedSide = Mathf.Lerp(smoothedSide, side, Time.deltaTime * 5f);
            MovePlayer();
        }
        if (isServer)
        {
            var dir = transform.forward;
            float dist = 1.5f;
            if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, dist))
            {
                print(hit.collider);
                if (hit.collider.TryGetComponent<Interactable>(out var inter))
                {
                    CurrentInteractable = inter;
                }
            } else
            {
                CurrentInteractable = null;
            }
        }

        if (CurrentInteractable != null)
        {
            var pos = CurrentInteractable.UIAnchor ? CurrentInteractable.UIAnchor.position : CurrentInteractable.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            InteractionText.transform.localScale = Vector3.one / (Vector3.Distance(Camera.main.transform.position, pos)/10);
            InteractionText.transform.position = screenPos;
            InteractionText.SetActive(true);
            if (string.IsNullOrEmpty(CurrentInteractable.InteractionText))
            {
                InteractionText.GetComponent<TextMeshProUGUI>().text = "Interact";
            } else
            {
                InteractionText.GetComponent<TextMeshProUGUI>().text = CurrentInteractable.InteractionText;
            }
        } else
        {
            InteractionText.SetActive(false);
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new(smoothedMove.x, 0, smoothedMove.y);

        var mag = Vector3.SqrMagnitude(movement);
        charControl.SimpleMove(movement);
        animator.SetFloat("Speed", mag);
        animator.SetFloat("Side", smoothedSide);

        if(movement != Vector3.zero)
        {  
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5f);
        }
    }
}
