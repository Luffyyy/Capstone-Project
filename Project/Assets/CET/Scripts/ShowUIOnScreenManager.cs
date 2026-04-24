using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShowUIOnScreenManager : MonoBehaviour
{
    public static ShowUIOnScreenManager Instance { get; private set; }
    public TextMeshProUGUI TextUI;
    public UnityEngine.UI.Image ImageToShow;
    public RectTransform uiRect;
    public RectTransform buttonRect;
    public UnityEngine.UI.Image ButtonImage;
    public UnityEngine.UI.Image PanelBackground;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        //RectTransform rectTransform = TextUI.rectTransform;
        HideUI();
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void ShowUI(Sprite spriteToShow, Vector3 rotation, Vector2 size, Vector2 buttonPosition, Color ButtonColor, string textUI, Vector2 textPosition, TMP_FontAsset TextFontAsset, float FontSize)
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.SetActive(false);
        }
        if (textUI != null)
        {
            TextUI.text = textUI;
            TextUI.rectTransform.anchoredPosition = textPosition;
            TextUI.font = TextFontAsset;
            TextUI.fontSize = FontSize;
        }
        if (ImageToShow != null && spriteToShow != null)
        {
            ImageToShow.sprite = spriteToShow;
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
        if (ButtonImage != null)
        {
            ButtonImage.color = ButtonColor;
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
