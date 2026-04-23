using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowUIOnScreenManager : MonoBehaviour
{
    public static ShowUIOnScreenManager Instance { get; private set; }
    public TextMeshProUGUI TextUI;
    public RawImage backgroundImage;
    public RectTransform uiRect;
    public RectTransform buttonRect; 


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

    public void ShowUI(Texture texture, Vector3 rotation, Vector2 size, Vector2 buttonPosition, string textUI)
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.SetActive(false);
        }
        if (textUI != null)
        {
            TextUI.text = textUI;
        }
        if (backgroundImage != null && texture != null)
        {
            backgroundImage.texture = texture;
        }
        if (uiRect != null)
        {
            uiRect.rotation = Quaternion.Euler(rotation);
            uiRect.sizeDelta = size;
        }
        if (buttonRect != null)
        {
            buttonRect.anchoredPosition = buttonPosition;
        }

        gameObject.SetActive(true);
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
