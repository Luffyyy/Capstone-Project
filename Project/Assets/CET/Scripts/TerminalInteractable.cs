using Mirror;
using UnityEngine;

public class TerminalInteractable : Interactable
{
    public GameObject VPLEditMenuPrefab;

    [HideInInspector]
    [SyncVar(hook="OnVPLMenuChanged")]
    public GameObject OwnedVPLMenu;

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
            OwnedVPLMenu.transform.SetParent(GameObject.Find("VPLMenu").transform, false);
        }
    }

    public override void Interact()
    {
        MenuManager.Instance.OpenMenu("VPLMenu");
        //TODO: Tell VPLMenu which VPLEditMenu we want to open
    }
}
