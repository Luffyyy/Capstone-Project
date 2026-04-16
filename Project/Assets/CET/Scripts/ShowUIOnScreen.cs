using Mirror.BouncyCastle.Crypto.Macs;
using UnityEngine;
using TMPro;

public class ShowUIOnScreen : Interactable
{

    public GameObject Canvas;
    public TextMeshProUGUI TextUI;
    public string TextToShow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        if (CurrentPlayer != null)
        {
            HUDManager.Instance.SetActive(false);
        }
        TextUI.text = TextToShow;
        Canvas.SetActive(true);
    }
    public void HideUI()
    {
        if(CurrentPlayer != null)
        {
            HUDManager.Instance.SetActive(true);
        }
        if (TextUI != null)
        {
            TextUI.text = "";
        }

        if (Canvas != null)
        {
            Canvas.SetActive(false);
        }
    }
}
