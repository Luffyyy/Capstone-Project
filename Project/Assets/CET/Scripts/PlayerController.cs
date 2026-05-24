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

    public bool IsInputEnabled = false;

    private CharacterController charControl;
    private Rigidbody rb;
    private Animator animator;
    public bool CanMove = false;

    public GameObject InteractionText;
    public GameObject InteractionTextPrefab;

    private AudioSource audioSource;

    public void SetInputEnabled(bool enabled)
    {
        IsInputEnabled = enabled;
        GetComponent<PlayerInput>().enabled = enabled;
        move = Vector2.zero; // Stop player
        side = 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !CanMove || MenuManager.Instance.IsActive) return;
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
            SetInputEnabled(true);

            CanMove = !GameState.Instance.InLobby();

            var op = GetComponent<PlayerInput>().currentActionMap.FindAction("OpenPauseMenu");
            op.performed += HUDManager.Instance.OpenPauseMenu;
            charControl.enabled = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !context.performed || MenuManager.Instance.IsActive || !IsInputEnabled) return;

        if (CurrentInteractable is Interactable i)
        {
            i.CmdInteract();
        }
    }
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;

        InteractionText = Instantiate(InteractionTextPrefab, GameObject.Find("Canvas").transform);
        InteractionText.transform.SetSiblingIndex(0);

        // Reset movement just in case
        animator.SetFloat("Speed", 0);
        animator.SetFloat("Side", 0);

    }
    void Update()
    {
        if (isLocalPlayer && charControl.enabled)
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
                if (hit.collider.TryGetComponent<Interactable>(out var inter) && inter.IsOn)
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
            InteractionText.transform.localScale = Vector3.one / Math.Min(1.75f, Vector3.Distance(Camera.main.transform.position, pos)/10);
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

        audioSource.enabled = movement.magnitude > 1f;

        if(movement != Vector3.zero)
        {  
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5f);
        }
    }
}
