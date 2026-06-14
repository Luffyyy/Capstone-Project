using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class ControlJournal : MenuBase
{
   public static ControlJournal Instance;
   public TextMeshProUGUI CollectablesFound;
   private int CollectablesCount = 0;
   public Image [] Slots;
   public Sprite[] CollectableSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }
    public void ApplyImage(CollectableType Type)
    {
        var index = (int)Type;
        CollectablesFound.text = $"{++CollectablesCount}/4 Items Found";
        Slots[index].sprite = CollectableSprites[index];
        Slots[index].GetComponent<Button>().interactable = true;
    }
    public void HideAndShow()
    {
        InteractionMenu.Instance.Show(InteractionMenu.Instance.Paper.sprite, InteractionMenu.Instance.TextUI.text, InteractionMenu.Instance.TextUI.rectTransform.offsetMin, InteractionMenu.Instance.TextUI.fontSize, InteractionMenu.Instance.TextUI.color, InteractionMenu.Instance.TextUI.font, InteractionMenu.Instance.DisplayOnPaper);
    }
}