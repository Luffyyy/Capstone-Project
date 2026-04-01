using Mirror;
using UnityEngine;

public class ShowUiOnTerminal : NetworkBehaviour
{
    private int playerCounter = 0;
    public GameObject promptUI;
    public Transform uiAnchor;
    
    public GameObject VPLEditMenuPrefab;

    [SyncVar(hook="OnVPLMenuChanged")]
    public GameObject OwnedVPLMenu;

    void PositionUI()
    {
        print(Camera.main);
        print(uiAnchor);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(uiAnchor.position);
        promptUI.transform.position = screenPos;
    }
    void Start()
    {
        if (isServer)
        {
            var go = Instantiate(VPLEditMenuPrefab, GameObject.Find("VPLMenu").transform);
            NetworkServer.Spawn(go);
            OwnedVPLMenu = go;
        }
    }

    void OnVPLMenuChanged(GameObject oldVPL, GameObject newVPL) {
        if (newVPL != null)
        {
            MenuManager.Instance.Show();
            OwnedVPLMenu.transform.SetParent(GameObject.Find("VPLMenu").transform, false);
        }
    }

    void OnConnectedToServer()
    {
        MenuManager.Instance.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + " entered the terminal area.");
            var player = other.GetComponent<TmpBasicMoves>();
            Debug.Log("Player component found: " + (player != null));
            if (player != null)
            {
                player.currentInteractable = this;
            }
            playerCounter++;
            PositionUI();
            promptUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<TmpBasicMoves>();
        if (player != null && player.currentInteractable == this)
        {
            player.currentInteractable = null;
        }

        playerCounter--;

        if (playerCounter == 0)
        {
            promptUI.SetActive(false);
        }
    }
    public void Interact()
    {
        Debug.Log("Interacted with ATM!");
        MenuManager.Instance.OpenMenu("VPLMenu");
        //TODO: Tell VPLMenu which VPLEditMenu we want to open
    }
}
