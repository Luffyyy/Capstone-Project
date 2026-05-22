using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowInteractionMenu : Interactable
{
    [Multiline]
    public string TextToShow;
    public Sprite SpriteToShow;
    public Vector2 TextPosition;
    public TMP_FontAsset FontAsset;
    public float FontSize = 90;
    public Color TextColor = Color.black;
    public GameObject DisplayOnPaper;

    public override void Interact()
    {
        base.Interact();
        if (InteractionMenu.Instance != null)
        {
            InteractionMenu.Instance.Show(SpriteToShow, TextToShow, TextPosition, FontSize, TextColor, FontAsset, DisplayOnPaper);
        }
    }
}
