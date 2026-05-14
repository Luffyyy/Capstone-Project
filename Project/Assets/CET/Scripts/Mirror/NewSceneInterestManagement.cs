using Mirror;
using UnityEngine;

public class NewSceneInterestManagement : SceneInterestManagement
{
    public override void SetHostVisibility(NetworkIdentity identity, bool visible)
    {
        // FORCE the server screen to ALWAYS show all renderers
        foreach (Renderer rend in identity.GetComponentsInChildren<Renderer>())
        {
            rend.enabled = true; 
        }
    }
}