using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class ControlJournal : MenuBase
{
    public static ControlJournal Instance;
    public TextMeshProUGUI CollectablesFound;
    private int CollectablesCount = 0;
    public Image[] Slots;
    public Sprite[] CollectableThumbs;
    public Sprite[] CollectableSprites;
    public AudioSource AudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }
    public void Collect(CollectableType Type)
    {

        var index = (int)Type;
        CollectablesFound.text = $"{++CollectablesCount}/4 Items Found";
        Slots[index].sprite = CollectableThumbs[index];
        Slots[index].GetComponent<Button>().interactable = true;
    }
    public void HideAndShow(int i)
    {
        AudioSource.Play();
        InteractionMenu.Instance.Show(CollectableSprites[i]);
    }
}