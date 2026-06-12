using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    public float speed;
    private Vector2 move;
    private Vector2 smoothedMove;
    private Vector3 previousPosition;
    private float footstepDistance;

    private const float FootstepSpacing = 1.25f;

    private float side;
    private float smoothedSide;

    [SyncVar]
    public Interactable CurrentInteractable;

    public bool IsInputEnabled = false;

    private CharacterController charControl;
    private Animator animator;
    public bool CanMove = false;

    public GameObject InteractionText;
    public GameObject InteractionTextPrefab;

    private AudioSource audioSource;

    public GameObject FocusPhone;

    [SyncVar]
    public bool IsFocusingOnPhone;

    private Player player;

    [Command]
    public void CmdSetFocusingOnPhone(bool isFocusing)
    {
        IsFocusingOnPhone = isFocusing;
    }

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

    public override void OnStartServer()
    {
        previousPosition = transform.position;
        footstepDistance = 0f;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !context.performed || MenuManager.Instance.IsActive || !IsInputEnabled) return;

        if (CurrentInteractable is Interactable i)
        {
            i.CmdInteract();
        }
    }
    public void OnJournalBtn(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer || !context.performed || MenuManager.Instance.IsActive || !IsInputEnabled) return;
        MenuManager.Instance.OpenMenu("Journal Menu");
    }
    void Awake()
    {
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();

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
            float radius = 1.25f;
            var origin = transform.position + Vector3.up;

            // Catch everything inside the starting sphere radius
            Collider[] colliders = Physics.OverlapSphere(origin, radius);
            bool found = false;
            foreach (var col in colliders)
            {
                if (col.CompareTag("BlockInteraction")) break;
                if (col.TryGetComponent<Interactable>(out var inter) && inter.IsOn && (inter.PlayerIndex == -1 || inter.PlayerIndex == player.PlayerIndex))
                {
                    found = true;
                    CurrentInteractable = inter;
                    break; // Grab the first one we find
                }
            }
            if (!found)
            {
                CurrentInteractable = null;
            }
        }

        if (isServer)
        {
            UpdateFootsteps();
        }

        if (CurrentInteractable != null)
        {
            var pos = CurrentInteractable.UIAnchor ? CurrentInteractable.UIAnchor.position : CurrentInteractable.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            InteractionText.transform.localScale = Vector3.one / Math.Min(2f, Vector3.Distance(Camera.main.transform.position, pos)/8);
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

        if (isServerOnly)
        {
            FocusPhone.SetActive(IsFocusingOnPhone);
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new(smoothedMove.x, 0, smoothedMove.y);

        var mag = Vector3.SqrMagnitude(movement);
        charControl.SimpleMove(movement);
        animator.SetFloat("Speed", mag);
        animator.SetFloat("speedMultiplier", 1.25f);
        animator.SetFloat("Side", smoothedSide);

        if(movement != Vector3.zero)
        {  
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5f);
        }
    }

    private void UpdateFootsteps()
    {
        if (audioSource == null)
        {
            return;
        }

        var positionDelta = Vector3.Distance(transform.position, previousPosition);
        footstepDistance += positionDelta;

        if (footstepDistance >= FootstepSpacing && positionDelta > 0.1f)
        {
            footstepDistance = 0f;
            PlayFootstepLocal();
            RpcPlayFootstep();
        }

        previousPosition = transform.position;
    }

    [ClientRpc]
    private void RpcPlayFootstep()
    {
        if (isServer)
        {
            return;
        }

        PlayFootstepLocal();
    }

    private void PlayFootstepLocal()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.PlayOneShot(audioSource.clip);
    }

    void OnDestroy()
    {
        Destroy(InteractionText);
    }
}
