using UnityEngine;

public class ShowUIOnScreen : Interactable
{
    public string TextToShow;

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
            ShowUIOnScreenManager.Instance.ShowText(TextToShow);
        }
    }
}
