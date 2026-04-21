using UnityEngine;
using TMPro;

public class ShowUIOnScreenManager : MonoBehaviour
{
    public static ShowUIOnScreenManager Instance { get; private set; }

    public TextMeshProUGUI TextUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        HideUI();
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void ShowText(string textToShow)
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.SetActive(false);
        }
        TextUI.text = textToShow;
        gameObject.SetActive(true);
        Debug.Log("Text should now be visible on screen");
    }

    public void HideUI()
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.SetActive(true);
        }
        if (TextUI != null)
        {
            TextUI.text = "";
        }
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
