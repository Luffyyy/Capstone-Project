using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShowCollectable : MonoBehaviour
{
    public Image ImageToShow;
    public TextMeshProUGUI Text;
    private int ItemsFound = 0;
    private Animator anim;
    public AudioSource audioSource;
    private void Awake()
    {
        gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }
    public void Show(Sprite sprite)
    {
        if (gameObject.activeSelf) return; // Don't show again, force hiding first
        gameObject.SetActive(true);
        ItemsFound++;
        Text.text = $"{ItemsFound}/4 Items Found!";
        ImageToShow.sprite = sprite;
        audioSource.Play();
        anim.Play("CollectedItem");
        MenuManager.Instance.CloseCurrentMenu(); // So players see it
        Invoke(nameof(Hide), 5f);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void HideAndShow()
    {
        InteractionMenu.Instance.Show(InteractionMenu.Instance.Paper.sprite, InteractionMenu.Instance.TextUI.text, InteractionMenu.Instance.TextUI.rectTransform.offsetMin, InteractionMenu.Instance.TextUI.fontSize, InteractionMenu.Instance.TextUI.color, InteractionMenu.Instance.TextUI.font, InteractionMenu.Instance.DisplayOnPaper);
    }
}