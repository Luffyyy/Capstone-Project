using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InteractionMenu : MenuBase
{
    public static InteractionMenu Instance { get; private set; }
    public TextMeshProUGUI TextUI;
    public Image Paper;
    
    public GameObject DisplayOnPaper;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

   void OnEnable()
   {
       UpdateWidth();
   }

   void OnRectTransformDimensionsChange()
   {
       UpdateWidth();
   }

    // Calculates correct width based on aspect ratio of the image allowing text to properly fit
    private void UpdateWidth()
    {
        RectTransform paperTransform = (RectTransform)Paper.transform;
        var size = paperTransform.sizeDelta;
        size.x = (transform as RectTransform).rect.height * Paper.preferredWidth/Paper.preferredHeight;
        paperTransform.sizeDelta = size;
    }

    public void Show(Sprite spriteToShow, string textUI, Vector2 textPosition, float fontSize, Color textColor, TMP_FontAsset textFontAsset=null, GameObject displayOnPaper=null)
    {
        if (Paper != null && spriteToShow != null)
        {
            Paper.sprite = spriteToShow;
        }

        if (textUI != null)
        {
            TextUI.text = textUI;
            TextUI.rectTransform.offsetMin = new Vector2(textPosition.x, 0);
            TextUI.rectTransform.offsetMax = new Vector2(0, textPosition.y);
            if (textFontAsset != null)
            {
                TextUI.font = textFontAsset;
            }
            TextUI.fontSize = fontSize;
            TextUI.color = textColor;
        }

        if (displayOnPaper != null)
        {
            DisplayOnPaper = Instantiate(displayOnPaper, Paper.transform);    
        }

        MenuManager.Instance.OpenMenu("InteractionMenu");
    }

    public override void Hide()
    {
        base.Hide();
        if (DisplayOnPaper != null)
        {
            Destroy(DisplayOnPaper);
            DisplayOnPaper = null;
        }
        if (TextUI != null)
        {
            TextUI.text = "";
        }
    }
}
