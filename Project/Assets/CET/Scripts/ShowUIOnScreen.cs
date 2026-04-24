using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowUIOnScreen : Interactable
{
    public string TextToShow;
    public Sprite SpriteToShow;
    public Vector3 Rotation;
    public Vector2 Size;
    public Color ButtonColor;
    public Vector2 ButtonPosition;
    public Vector2 TextPosition;
    public TMP_FontAsset FontAsset;
    public float FontSize;

    void Start()
    {
    }

    void Update()
    {
    }

    public override void Interact()
    {
        if (ShowUIOnScreenManager.Instance != null)
        {
            ShowUIOnScreenManager.Instance.ShowUI(SpriteToShow, Rotation, Size, ButtonPosition, ButtonColor, TextToShow, TextPosition, FontAsset, FontSize);
        }
    }
}
