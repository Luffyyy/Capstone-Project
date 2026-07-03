using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

[ExecuteInEditMode]
public class InteractionMenu : MenuBase
{
    public static InteractionMenu Instance { get; private set; }
    public TextMeshProUGUI TextUI;
    public Image Paper;
    public Sprite DefaultPaperSprite;

    public GameObject DisplayOnPaper;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        DefaultPaperSprite = Paper.sprite;
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
        size.x = (transform as RectTransform).rect.height * Paper.preferredWidth / Paper.preferredHeight;
        paperTransform.sizeDelta = size;
    }

    public void Show(Sprite spriteToShow, GameObject displayOnPaper = null)
    {
        Paper.sprite = spriteToShow != null ? spriteToShow : DefaultPaperSprite;

        UpdateWidth();

        if (displayOnPaper != null)
        {
            DisplayOnPaper = Instantiate(displayOnPaper, Paper.transform);
        }

        MenuManager.Instance.OpenMenu("InteractionMenu");

        if (NetworkClient.localPlayer != null)
            NetworkClient.localPlayer.GetComponent<PlayerController>().CmdSetFocusingOnPhone(true);
    }

    public void Show(Sprite spriteToShow, string text, Vector2? textPosition, float fontSize, Color color, TMP_FontAsset font = null, GameObject displayOnPaper = null)
    {
        if (text != null)
        {
            TextUI.text = text;
            if (textPosition is Vector2 tp)
            {
                TextUI.rectTransform.offsetMax = new Vector2(0, tp.y);
                TextUI.rectTransform.offsetMin = new Vector2(tp.x, 0);
            }
            if (font != null)
            {
                TextUI.font = font;
            }
            TextUI.fontSize = fontSize;
            TextUI.color = color;
        }

        Show(spriteToShow, displayOnPaper);
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

        if (NetworkClient.localPlayer != null)
            NetworkClient.localPlayer.GetComponent<PlayerController>().CmdSetFocusingOnPhone(false);
    }
}
