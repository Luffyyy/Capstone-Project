using UnityEngine;
using TMPro;
using Mirror;
public class RevealObj : NetworkBehaviour
{
    [SyncVar(hook = nameof(CallReveal))] public bool IsRevealed;
    public TextMeshProUGUI RevealObject;
    public GameObject ObjectToReveal;
    public string TextToReveal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStartClient()
    {
        base.OnStartClient();
        SetReveal(IsRevealed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      private void CallReveal(bool oldValue, bool newValue)
    {
        SetReveal(newValue);
    }
    public void SetReveal(bool value)
    {
        if (value)
        {
            if(RevealObject != null)
            {
                RevealObject.gameObject.SetActive(true);
            }
            if(ObjectToReveal != null)
            {
                ObjectToReveal.SetActive(true);
            }
        }
        else
        {
            if(RevealObject != null)
            {
                RevealObject.gameObject.SetActive(false);
            }
            if(ObjectToReveal != null)
            {
                ObjectToReveal.SetActive(false);
            }
        }
    }
}
