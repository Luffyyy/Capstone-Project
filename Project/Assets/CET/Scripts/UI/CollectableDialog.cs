using UnityEngine;
using UnityEngine.UI;
public class CollectableDialog : Dialog
{
    public Image ImageToShow;

    public void Show(Sprite image)
    {
        base.Show();
        if (image != null)
        {
            ImageToShow.sprite = image;
        }
    }
    public override void Hide()
    {
        base.Hide();
        ImageToShow.sprite = null;
    }
    public void HideAndShow()
    {
        base.Hide();
        InteractionMenu.Instance.Show(InteractionMenu.Instance.Paper.sprite, InteractionMenu.Instance.TextUI.text, InteractionMenu.Instance.TextUI.rectTransform.offsetMin, InteractionMenu.Instance.TextUI.fontSize, InteractionMenu.Instance.TextUI.color, InteractionMenu.Instance.TextUI.font, InteractionMenu.Instance.DisplayOnPaper);
    }
}