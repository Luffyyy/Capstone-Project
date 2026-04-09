using System.ComponentModel.Design;
using Mirror;
using UnityEngine;

public class Interactable : Activatable
{
    private int playerCounter = 0;
    public GameObject PromptUI;
    public Transform UIAnchor;
    public GameObject PromptUIPrefab;
    public void Awake()
    {
        PromptUI = Instantiate(PromptUIPrefab, GameObject.Find("Canvas").transform);
        PromptUI.transform.SetSiblingIndex(0);
    }
    protected void PositionUI()
    {
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(UIAnchor.position);
        PromptUI.transform.position = screenPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (IsOn)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CurrentInteractable = this;
            }

            playerCounter++;
            PositionUI();
            PromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player != null && player.CurrentInteractable == this)
        {
            player.CurrentInteractable = null;
        }

        playerCounter--;

        if (playerCounter == 0)
        {
            PromptUI.SetActive(false);
        }
    }
    public virtual void Interact()
    {
        
    }
}
